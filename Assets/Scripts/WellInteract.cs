//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro;
//using UnityEngine.Video; 

//[RequireComponent(typeof(Collider2D))]
//public class WellInteract : MonoBehaviour
//{
//    public GameObject interactTooltipUI;
//    public string tooltipText = "Press [Space] to look into the well";

//    [Header("Cài đặt Video Chuyển Cảnh")]
//    public VideoPlayer videoPlayer; 
//    public string nextScene = "Lv3_1_LuaDau"; 

//    private bool isPlayerNear = false;
//    private bool isDone = false;

//    void Start()
//    {
//        GetComponent<Collider2D>().isTrigger = true;
//    }

//    void Update()
//    {
//        if (isPlayerNear && !isDone && Input.GetKeyDown(KeyCode.Space))
//        {
//            // NẾU DÌ GHẺ CHƯA BỊ LỪA -> KHÔNG CHO KHÁM GIẾNG
//            if (DiGheAI.isInvestigating == false)
//            {
//                string[] lines = { "Tam: (Thinking) Stepmother is still guarding it. I need to distract her first!" };
//                if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);
//                return;
//            }

//            // NẾU ĐÃ LỪA ĐƯỢC DÌ GHẺ -> BẮT ĐẦU SỰ KIỆN END LEVEL!
//            isDone = true;
//            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

//            StartCoroutine(PlayEndCutsceneRoutine());
//        }
//    }

//    IEnumerator PlayEndCutsceneRoutine()
//    {
//        string[] linesSuccess = {
//            "Tam: Bong Bong bang bang... come eat the golden rice...",
//            "System: ... (Silence) ...",
//            "Tam: Bong? Where are you? ... Wait, what is this red stain on the water?",
//            "System: The fish is dead. The stepmother killed it.",
//            "Tam: NOOOOO!!! *cries*"
//        };

//        // 1. Mở thoại
//        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(linesSuccess);

//        // 2. Chờ 0.5s cho an toàn, rồi chờ đến khi người chơi tắt hộp thoại
//        yield return new WaitForSeconds(0.5f);
//        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

//        // 3. Ẩn UI và Nhân vật đi để Video hiện lên cho đẹp
//        GameObject hud = GameObject.Find("GameHUD");
//        if (hud != null) hud.SetActive(false);

//        GameObject player = GameObject.FindGameObjectWithTag("Player");
//        if (player != null) player.SetActive(false);

//        // 4. Phát Video!
//        if (videoPlayer != null)
//        {
//            videoPlayer.Play();

//            // Lệnh này bắt game chờ đúng bằng số giây của đoạn Video
//            yield return new WaitForSeconds((float)videoPlayer.length + 0.5f);
//        }

//        // 5. Kết thúc Video -> Chuyển sang màn 3
//        Debug.Log(">>> END LEVEL 2! ĐANG CHUYỂN SANG LEVEL 3... <<<");
//        SceneManager.LoadScene(nextScene);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player") && !isDone)
//        {
//            isPlayerNear = true;
//            if (interactTooltipUI != null)
//            {
//                interactTooltipUI.GetComponentInChildren<TextMeshProUGUI>().text = tooltipText;
//                interactTooltipUI.SetActive(true);
//            }
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            isPlayerNear = false;
//            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
//        }
//    }
//}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

[RequireComponent(typeof(Collider2D))]
public class WellInteract : MonoBehaviour
{
    public GameObject interactTooltipUI;
    public string tooltipText = "Press [Space] to look into the well";

    [Header("Cài đặt Video Chuyển Cảnh")]
    public VideoPlayer videoPlayer;

    // Đã đổi tên Load sang màn Phép Màu của Level 3 nhé!
    public string nextScene = "Lv3_1_PhepMau";

    private bool isPlayerNear = false;
    private bool isDone = false;

    // === THÊM BIẾN CHỐNG DÍNH PHÍM ===
    private float interactCooldown = 0f;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Update()
    {
        // Trừ dần thời gian chờ
        if (interactCooldown > 0) interactCooldown -= Time.deltaTime;

        if (isPlayerNear)
        {
            // NẾU ĐANG NÓI CHUYỆN -> Khóa phím 0.2 giây và ẩn Tooltip
            if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
            {
                if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
                interactCooldown = 0.2f;
            }
            else
            {
                // NẾU KHÔNG NÓI CHUYỆN -> Bật Tooltip
                if (interactTooltipUI != null && !isDone) interactTooltipUI.SetActive(true);

                // CHỈ CHO PHÉP TƯƠNG TÁC KHI COOLDOWN ĐÃ VỀ 0
                if (interactCooldown <= 0f && !isDone && Input.GetKeyDown(KeyCode.Space))
                {
                    if (DiGheAI.isInvestigating == false)
                    {
                        string[] lines = { "Tam: (Thinking) Stepmother is still guarding it. I need to distract her first!" };
                        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);
                    }
                    else
                    {
                        isDone = true;
                        if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
                        StartCoroutine(PlayEndCutsceneRoutine());
                    }
                }
            }
        }
    }

    IEnumerator PlayEndCutsceneRoutine()
    {
        string[] linesSuccess = {
            "Tam: Bong Bong bang bang... come eat the golden rice...",
            "System: ... (Silence) ...",
            "Tam: Bong? Where are you? ... Wait, what is this red stain on the water?",
            "System: The fish is dead. The stepmother killed it.",
            "Tam: NOOOOO!!! *cries*"
        };

        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(linesSuccess);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

        GameObject hud = GameObject.Find("GameHUD");
        if (hud != null) hud.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            yield return new WaitForSeconds((float)videoPlayer.length + 0.5f);
        }

        Debug.Log(">>> END LEVEL 2! ĐANG CHUYỂN SANG LEVEL 3... <<<");
        SceneManager.LoadScene(nextScene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDone)
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
        }
    }
}