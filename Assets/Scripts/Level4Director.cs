using System.Collections;
using UnityEngine;

public class Level4Director : MonoBehaviour
{
    [Header("Nhân vật Intro")]
    public GameObject tamAoMoi;
    public GameObject xichTho;

    [Header("Nhân vật Chơi chính")]
    public GameObject tamCuoiNgua;

    [Header("UI Hướng dẫn")]
    public GameObject tutorialText; // Kéo cái chữ TutorialText vào đây

    public float tocDoDiBo = 3f;

    // Biến toàn cục quyết định game đã chạy chưa
    public static bool isPlaying = false;

    void Start()
    {
        isPlaying = false;
        tamCuoiNgua.SetActive(false); // Ẩn Tấm cưỡi ngựa đi
        StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        // THÊM DÒNG NÀY ĐỂ ĐỢI GAMEHUD KHỞI TẠO XONG RỒI MỚI NÓI
        yield return new WaitForSeconds(0.5f);

        // 1. Thoại mở đầu
        string[] lines = {
            "Tam: Oh no, I'm late for the King's festival!",
            "System: Xich Tho has joined your party!",
            "Tam: Let's go, my trusty steed!"
        };
        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);
        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

        // =====================================
        // 2. BẬT ANIMATION VÀ ĐI BỘ LẠI GẦN NGỰA
        // =====================================
        Animator tamAnim = tamAoMoi.GetComponent<Animator>();
        if (tamAnim != null) tamAnim.SetBool("isMove", true); // Kích hoạt biến isMove của bạn!

        Vector3 targetPos = xichTho.transform.position;
        while (Vector3.Distance(tamAoMoi.transform.position, targetPos) > 0.1f)
        {
            tamAoMoi.transform.position = Vector3.MoveTowards(tamAoMoi.transform.position, targetPos, tocDoDiBo * Time.deltaTime);
            yield return null;
        }

        // Tắt animation đi bộ (Dù sao thì ngay sau đó Tấm cũng tàng hình, nhưng cứ tắt cho chuẩn)
        if (tamAnim != null) tamAnim.SetBool("isMove", false);

        // 3. Ma thuật Lên Ngựa!
        tamAoMoi.SetActive(false);
        xichTho.SetActive(false);

        tamCuoiNgua.transform.position = xichTho.transform.position;
        tamCuoiNgua.SetActive(true);

        yield return new WaitForSeconds(1f);

        // 4. BẮT ĐẦU GAME!
        isPlaying = true;

        // HIỆN HƯỚNG DẪN 5 GIÂY RỒI TẮT
        if (tutorialText != null) tutorialText.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (tutorialText != null) tutorialText.SetActive(false);
    }
}