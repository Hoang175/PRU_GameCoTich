//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Level3Controller : MonoBehaviour
//{
//    [Header("Giao diện & Nhân vật")]
//    public GameObject taskCanvas;
//    public GameObject tamPlayer;

//    [Header("Đồ mới của Tấm")]
//    public Sprite tamNewClothes;

//    void Start()
//    {
//        if (taskCanvas != null) taskCanvas.SetActive(false);
//        StartCoroutine(IntroRoutine());
//    }

//    IEnumerator IntroRoutine()
//    {
//        // Chờ 0.5s cho UI load ổn định
//        yield return new WaitForSeconds(0.5f);

//        // THOẠI TIẾNG ANH ĐẦU GAME
//        string[] introDialogues = {
//            "Tam: Stepmother forced me to sort all the rice... The sparrows helped me, but I have nothing to wear to the festival!",
//            "Fairy: Do not cry. I have hidden a Dress, a Scarf, Shoes, and a Wooden Horse around the yard. Find them!",
//            "System: Use your mouse (Click) to find 4 hidden items. Drag and drop obstacles to reveal hidden spots."
//        };

//        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(introDialogues);

//        // Chờ người chơi bấm Space đọc xong
//        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

//        // Đọc xong thì bung cái khung Taskbar ra cho người chơi tìm đồ
//        if (taskCanvas != null) taskCanvas.SetActive(true);
//    }

//    public void TransformTam()
//    {
//        StartCoroutine(TransformRoutine());
//    }

//    IEnumerator TransformRoutine()
//    {
//        if (taskCanvas != null) taskCanvas.SetActive(false);

//        // THOẠI TIẾNG ANH KHI THẮNG
//        string[] winLines = {
//            "Tam: Wow! The dress and the horse are so beautiful! Thank you, Fairy!",
//            "Fairy: Put them on and go to the festival. But remember to return before midnight!",
//            "System: Level 3 Complete! Prepare for the Festival!"
//        };

//        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(winLines);
//        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

//        // Tấm Biến Hình
//        Animator tamAnim = tamPlayer.GetComponent<Animator>();
//        if (tamAnim != null) tamAnim.enabled = false;

//        SpriteRenderer tamSR = tamPlayer.GetComponent<SpriteRenderer>();
//        if (tamSR != null) tamSR.sprite = tamNewClothes;

//        yield return new WaitForSeconds(2f); // Chờ 2s ngắm đồ

//        Debug.Log(">>> CHUYỂN SANG MÀN 4... <<<");
//        // Tạm khóa dòng chuyển màn lại, chừng nào làm map Lv 4 thì mở ra nhé:
//        // SceneManager.LoadScene("Lv4_1_TrayHoi"); 
//    }
//}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // THÊM DÒNG NÀY ĐỂ MỞ TÍNH NĂNG VIDEO

public class Level3Controller : MonoBehaviour
{
    [Header("Giao diện & Nhân vật")]
    public GameObject taskCanvas;
    public GameObject tamPlayer;

    [Header("Đồ mới của Tấm")]
    //public Sprite tamNewClothes;
    public GameObject tamNewPrefab;

    [Header("Video Kết Màn 3")]
    public VideoPlayer videoPlayer; // KÉO OBJECT "CutscenePlayer" VÀO ĐÂY
    public string nextScene = "Lv4_1_TrayHoi"; // Tên Màn 4 sắp làm

    void Start()
    {
        if (taskCanvas != null) taskCanvas.SetActive(false);
        StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        string[] introDialogues = {
            "Tam: Stepmother forced me to sort all the rice... The sparrows helped me, but I have nothing to wear to the festival!",
            "Fairy: Do not cry. I have hidden a Dress, a Scarf, Shoes, and a Wooden Horse around the yard. Find them!",
            "System: Use your mouse (Click) to find 4 hidden items. Drag and drop obstacles to reveal hidden spots."
        };

        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(introDialogues);
        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

        if (taskCanvas != null) taskCanvas.SetActive(true);
    }

    public void TransformTam()
    {
        StartCoroutine(TransformRoutine());
    }

    IEnumerator TransformRoutine()
    {
        if (taskCanvas != null) taskCanvas.SetActive(false);

        string[] winLines = {
            "Tam: Wow! The dress and the horse are so beautiful! Thank you, Fairy!",
            "Fairy: Put them on and go to the festival. But remember to return before midnight!",
            "System: Level 3 Complete! Prepare for the Festival!"
        };

        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(winLines);
        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

        // ============================
        // TẤM BIẾN HÌNH
        // ============================
        //Animator tamAnim = tamPlayer.GetComponent<Animator>();
        //if (tamAnim != null) tamAnim.enabled = false;

        //SpriteRenderer tamSR = tamPlayer.GetComponent<SpriteRenderer>();
        //if (tamSR != null) tamSR.sprite = tamNewClothes;

        //yield return new WaitForSeconds(2f); // Dừng 2 giây để người chơi ngắm bộ đồ lộng lẫy
        // ============================
        // TẤM BIẾN HÌNH (DÙNG PREFAB ANIMATION)
        // ============================

        // Lấy vị trí của Tấm cũ
        Vector3 oldPosition = tamPlayer.transform.position;

        // Ẩn Tấm cũ đi vĩnh viễn
        if (tamPlayer != null) tamPlayer.SetActive(false);

        // Triệu hồi Tấm mới (Đẹp, có Animation) ngay tại vị trí Tấm cũ!
        if (tamNewPrefab != null)
        {
            Instantiate(tamNewPrefab, oldPosition, Quaternion.identity);

            // Tùy chọn: Thêm tiếng "Bùm!" hoặc Particle System phép thuật ở đây
        }

        yield return new WaitForSeconds(2f); // Dừng 2 giây để ngắm Tấm mới nhún nhảy

        // ... [Đoạn code tắt HUD, phát Video và Chuyển màn giữ nguyên] ...

        // ============================
        // TẮT HẾT ĐỒ ĐẠC ĐỂ CHIẾU VIDEO
        // ============================
        GameObject hud = GameObject.Find("GameHUD");
        if (hud != null) hud.SetActive(false);

        if (tamPlayer != null) tamPlayer.SetActive(false);
        GameObject butNPC = GameObject.Find("ButNPC");
        if (butNPC != null) butNPC.SetActive(false);

        // PHÁT VIDEO CHUYỂN CẢNH
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            // Bắt game đứng chờ đúng bằng thời lượng của cái Video (cộng thêm 0.5s cho mượt)
            yield return new WaitForSeconds((float)videoPlayer.length + 0.5f);
        }

        // BAY SANG MÀN 4
        Debug.Log(">>> ĐÃ CHIẾU XONG VIDEO! CHUYỂN SANG MÀN 4... <<<");
        SceneManager.LoadScene(nextScene);
    }
}