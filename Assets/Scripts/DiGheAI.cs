using System.Collections;
using UnityEngine;

public class DiGheAI : MonoBehaviour
{
    [Header("Cài đặt Tuần tra")]
    public Transform[] waypoints;
    public float speed = 2f;

    [Header("Vùng Tầm Nhìn")]
    public Transform visionZone;

    private int currentPoint = 0;
    private Animator anim;
    private SpriteRenderer sr;

    // === CÁC BIẾN MỚI CHO TRÍ TUỆ NHÂN TẠO (ĐÁNH LẠC HƯỚNG) ===
    //private bool isInvestigating = false; // Có đang đi kiểm tra tiếng động không?
    public static bool isInvestigating = false;
    private Vector2 soundLocation;        // Vị trí tiếng Keng phát ra
    private bool isWaitingAtSound = false;// Đứng im ngó nghiêng khi đến chỗ cái chum

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Nếu chưa gắn Waypoint hoặc đang đứng im kiểm tra thì không chạy code di chuyển
        if (waypoints.Length == 0 || isWaitingAtSound) return;

        // BỘ NÃO QUYẾT ĐỊNH: Đi tới tiếng động HAY đi tuần tra điểm tiếp theo?
        Vector2 targetPos = isInvestigating ? soundLocation : (Vector2)waypoints[currentPoint].position;

        // Di chuyển Dì Ghẻ
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        Vector2 direction = targetPos - (Vector2)transform.position;

        // Kiểm tra xem đã đến đích chưa (sai số 0.1f)
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            if (isInvestigating)
            {
                // Nếu đi kiểm tra tiếng động mà đến nơi rồi -> Đứng lại ngó nghiêng 3 giây!
                StartCoroutine(WaitAndResumePatrol());
            }
            else
            {
                // Nếu đang đi tuần mà đến nơi rồi -> Đổi sang điểm tuần tra tiếp theo
                currentPoint++;
                if (currentPoint >= waypoints.Length) currentPoint = 0;
            }
        }
        else
        {
            // Cập nhật Animation lúc đang đi
            UpdateAnimationAndVision(direction);
        }
    }

    // ========================================================
    // ĐÂY CHÍNH LÀ CÁI "LỖ TAI" MÀ SCRIPT CHỌI CHUM ĐANG TÌM GỌI!
    // ========================================================
    public void InvestigateSound(Vector2 pos)
    {
        if (!isInvestigating) // Tránh trường hợp ném 2 lần bả lú
        {
            isInvestigating = true;  // Bật chế độ nghi ngờ
            soundLocation = pos;     // Lưu vị trí cái chum lại
            isWaitingAtSound = false;
        }
    }

    // Dì Ghẻ đứng lại kiểm tra, sau 3 giây không có gì thì quay lại đi tuần
    IEnumerator WaitAndResumePatrol()
    {
        isWaitingAtSound = true; // Dừng bước

        // Tắt animation đi bộ, đứng im thở dốc
        anim.SetBool("isRun", false);
        anim.SetBool("isMoveUp", false);
        anim.SetBool("isMoveDown", false);

        // ĐỢI 3 GIÂY CHO TẤM CHẠY RA GIẾNG!
        yield return new WaitForSeconds(3f);

        // Ngó nghiêng xong chả thấy ai, bả quay về đi tuần tiếp
        isInvestigating = false;
        isWaitingAtSound = false;
    }

    void UpdateAnimationAndVision(Vector2 dir)
    {
        anim.SetBool("isRun", false);
        anim.SetBool("isMoveUp", false);
        anim.SetBool("isMoveDown", false);

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            anim.SetBool("isRun", true);
            if (dir.x > 0)
            {
                sr.flipX = true; // Đi PHẢI
                if (visionZone != null) visionZone.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                sr.flipX = false;  // Đi TRÁI
                if (visionZone != null) visionZone.localRotation = Quaternion.Euler(0, 0, 180);
            }
        }
        else
        {
            if (dir.y > 0)
            {
                anim.SetBool("isMoveUp", true);
                if (visionZone != null) visionZone.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                anim.SetBool("isMoveDown", true);
                if (visionZone != null) visionZone.localRotation = Quaternion.Euler(0, 0, -90);
            }
        }
    }
}