using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyVision : MonoBehaviour
{
    [Header("Cài đặt Tầm Nhìn")]
    public LayerMask obstacleLayer;
    public string gameOverScene = "Lv2_1_DuongVao";
    public AudioClip alertSound;

    // === THÊM MỚI: Biến để kéo object [!] vào ===
    [Header("UI Cảnh Báo")]
    public GameObject alertIconObject;

    private SpriteRenderer parentSprite;
    private bool isCaught = false;

    void Start()
    {
        parentSprite = GetComponentInParent<SpriteRenderer>();

        // === THÊM MỚI: Đảm bảo [!] bị ẩn lúc đầu (để chắc chắn) ===
        if (alertIconObject != null) alertIconObject.SetActive(false);
    }

    void Update()
    {
        if (parentSprite != null)
        {
            float direction = parentSprite.flipX ? 1f : -1f;
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isCaught) return;

        if (collision.CompareTag("Player"))
        {
            Vector2 enemyPos = transform.parent.position;
            Vector2 playerPos = collision.transform.position;
            Vector2 directionToPlayer = playerPos - enemyPos;
            float distanceToPlayer = directionToPlayer.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(enemyPos, directionToPlayer, distanceToPlayer, obstacleLayer);

            if (hit.collider == null)
            {
                StartCoroutine(CatchPlayerRoutine());
            }
        }
    }

    IEnumerator CatchPlayerRoutine()
    {
        isCaught = true;

        // === THÊM MỚI: Bật dấu [!] lên ngay lập tức ===
        if (alertIconObject != null) alertIconObject.SetActive(true);

        if (alertSound != null) AudioSource.PlayClipAtPoint(alertSound, transform.position);

        string[] lines = {
            "Cám: Á à! Chị Tấm kia! Dì bảo chị đi chăn trâu sao dám lén mò về nhà hả?",
            "Tấm: Thôi chết... bị Cám phát hiện rồi...",
            "Hệ thống: Bạn đã bị tóm! Nhấn [Space] để làm lại cuộc đời."
        };

        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(lines);
        }

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

        SceneManager.LoadScene(gameOverScene);
    }
}

//using System.Collections; // Bắt buộc thêm để dùng Coroutine
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class EnemyVision : MonoBehaviour
//{
//    [Header("Cài đặt Tầm Nhìn")]
//    public LayerMask obstacleLayer;
//    public string gameOverScene = "Lv2_1_DuongVao";
//    public AudioClip alertSound; // Tiếng giật mình (Teng!!!)

//    private SpriteRenderer parentSprite;
//    private bool isCaught = false; // Biến chống spam chửi bới

//    void Start()
//    {
//        parentSprite = GetComponentInParent<SpriteRenderer>();
//    }

//    void Update()
//    {
//        if (parentSprite != null)
//        {
//            float direction = parentSprite.flipX ? 1f : -1f;
//            transform.localScale = new Vector3(direction, 1, 1);
//        }
//    }

//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        if (isCaught) return; // Bị tóm rồi thì không quét nữa

//        if (collision.CompareTag("Player"))
//        {
//            Vector2 enemyPos = transform.parent.position;
//            Vector2 playerPos = collision.transform.position;
//            Vector2 directionToPlayer = playerPos - enemyPos;
//            float distanceToPlayer = directionToPlayer.magnitude;

//            RaycastHit2D hit = Physics2D.Raycast(enemyPos, directionToPlayer, distanceToPlayer, obstacleLayer);

//            if (hit.collider == null)
//            {
//                // Gọi chuỗi sự kiện bắt quả tang!
//                StartCoroutine(CatchPlayerRoutine());
//            }
//        }
//    }

//    IEnumerator CatchPlayerRoutine()
//    {
//        isCaught = true; // Khóa lại, bắt 1 lần thôi

//        if (alertSound != null) AudioSource.PlayClipAtPoint(alertSound, transform.position);

//        string[] lines = {
//            "Cám: Á à! Chị Tấm kia! Dì bảo chị đi chăn trâu sao dám lén mò về nhà hả?",
//            "Tấm: Thôi chết... bị Cám phát hiện rồi...",
//            "Hệ thống: Bạn đã bị tóm! Nhấn [Space] để làm lại cuộc đời."
//        };

//        if (DialogueManager.instance != null)
//        {
//            DialogueManager.instance.StartDialogue(lines);
//        }

//        // Đợi 0.5 giây cho khung thoại kịp bật lên
//        yield return new WaitForSeconds(0.5f);

//        // CÚ CHỐT: Yêu cầu Game CHỜ ĐỢI cho đến khi Khung thoại tắt đi thì mới chạy tiếp
//        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

//        // Đọc xong chữ, khung thoại tắt -> Bùm! Chuyển cảnh.
//        SceneManager.LoadScene(gameOverScene);
//    }
//}