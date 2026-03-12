//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro; // THÊM DÒNG NÀY

//[RequireComponent(typeof(Collider2D))]
//public class LockedGateInteract : MonoBehaviour
//{
//    // ... [GIỮ NGUYÊN CÁC BIẾN CŨ CỦA BẠN] ...
//    [Header("Cài đặt Cổng Khóa")]
//    public string nextScene = "Lv2_3_SanSau";
//    public AudioClip lockedSound;
//    public AudioClip unlockSound;
//    public float delayTime = 1f;

//    public GameObject interactTooltipUI;

//    // THÊM CÁI NÀY:
//    public string tooltipText = "Nhấn [Space] để mở khóa";

//    private AudioSource audioSource;
//    private bool isPlayerNear = false;
//    private bool isTeleporting = false;

//    void Start()
//    {
//        GetComponent<Collider2D>().isTrigger = true;
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
//    }

//    void Update()
//    {
//        if (isPlayerNear && Input.GetKeyDown(KeyCode.Space) && !isTeleporting)
//        {
//            TryOpenGate();
//        }
//    }

//    void TryOpenGate()
//    {
//        if (KeyItem.hasKey == false)
//            //if (DongRomInteract.hasKey == false)
//        {
//            if (lockedSound != null) audioSource.PlayOneShot(lockedSound);
//            string[] lines = { "Tấm: Cổng bị khóa chặt rồi. Chắc Dì ghẻ giấu chìa khóa ở đâu đó quanh sân này..." };
//            if (DialogueManager.instance != null && !DialogueManager.instance.isTalking)
//            {
//                DialogueManager.instance.StartDialogue(lines);
//            }
//        }
//        else
//        {
//            // Mở được cổng thì tắt chữ Tooltip đi
//            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
//            StartCoroutine(OpenAndTeleport());
//        }
//    }

//    // ... [GIỮ NGUYÊN Hàm OpenAndTeleport NHƯ CŨ] ...
//    IEnumerator OpenAndTeleport()
//    {
//        isTeleporting = true;
//        if (unlockSound != null) audioSource.PlayOneShot(unlockSound);
//        string[] lines = { "Tấm: Mở được rồi! Phải mau lẻn ra giếng xem bé Bống thế nào!" };
//        if (DialogueManager.instance != null && !DialogueManager.instance.isTalking)
//        {
//            DialogueManager.instance.StartDialogue(lines);
//        }
//        yield return new WaitForSeconds(delayTime);
//        SceneManager.LoadScene(nextScene);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            isPlayerNear = true;
//            if (!isTeleporting && interactTooltipUI != null)
//            {
//                // ĐỔI CHỮ THÀNH "MỞ KHÓA"
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

[RequireComponent(typeof(Collider2D))]
public class LockedGateInteract : MonoBehaviour
{
    [Header("Cài đặt Cổng Khóa")]
    public string nextScene = "Lv2_3_SanSau";
    public AudioClip lockedSound;
    public AudioClip unlockSound;
    public float delayTime = 1f;

    public GameObject interactTooltipUI;
    public string tooltipText = "Nhấn [Space] để mở khóa";

    private AudioSource audioSource;
    private bool isPlayerNear = false;
    private bool isTeleporting = false;

    // === BỘ ĐẾM CHỐNG DÍNH PHÍM ===
    private float interactCooldown = 0f;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Giảm dần thời gian chờ
        if (interactCooldown > 0) interactCooldown -= Time.deltaTime;

        if (isPlayerNear)
        {
            // Nếu đang nói chuyện -> Giữ Cooldown luôn ở mức 0.2s và ẩn Tooltip
            if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
            {
                if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
                interactCooldown = 0.2f;
            }
            else
            {
                // Nếu không nói chuyện + chưa dịch chuyển -> Bật Tooltip
                if (interactTooltipUI != null && !isTeleporting) interactTooltipUI.SetActive(true);

                // CHỈ CHO BẤM KHI COOLDOWN ĐÃ VỀ 0
                if (interactCooldown <= 0f && Input.GetKeyDown(KeyCode.Space) && !isTeleporting)
                {
                    TryOpenGate();
                }
            }
        }
    }

    void TryOpenGate()
    {
        if (KeyItem.hasKey == false)
        {
            if (lockedSound != null) audioSource.PlayOneShot(lockedSound);
            string[] lines = { "Tấm: Cổng bị khóa chặt rồi. Chắc Dì ghẻ giấu chìa khóa ở đâu đó quanh sân này..." };
            if (DialogueManager.instance != null && !DialogueManager.instance.isTalking)
            {
                DialogueManager.instance.StartDialogue(lines);
            }
        }
        else
        {
            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
            StartCoroutine(OpenAndTeleport());
        }
    }

    IEnumerator OpenAndTeleport()
    {
        isTeleporting = true;
        if (unlockSound != null) audioSource.PlayOneShot(unlockSound);
        string[] lines = { "Tấm: Mở được rồi! Phải mau lẻn ra giếng xem bé Bống thế nào!" };
        if (DialogueManager.instance != null && !DialogueManager.instance.isTalking)
        {
            DialogueManager.instance.StartDialogue(lines);
        }
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(nextScene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (!isTeleporting && interactTooltipUI != null)
            {
                interactTooltipUI.GetComponentInChildren<TextMeshProUGUI>().text = tooltipText;
            }
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