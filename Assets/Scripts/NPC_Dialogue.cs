//using UnityEngine;

//[RequireComponent(typeof(Collider2D))]
//public class NPC_Dialogue : MonoBehaviour
//{
//    [Header("Noi dung hop thoai")]
//    [TextArea(2, 5)] // Giúp ô nhập chữ ngoài Unity to ra cho dễ gõ
//    public string[] lines;

//    [Header("Giao dien & am thanh")]
//    public GameObject interactTooltipUI; // Kéo chữ "Bấm Space" vào đây
//    public AudioClip talkSound;          // Tiếng "Ting" khi nói chuyện
//    private AudioSource audioSource;

//    private bool isPlayerNear = false;

//    // Biến tĩnh dùng chung để báo cho Cánh Cửa biết là Tấm đã nhận nhiệm vụ
//    public static bool hasReceivedQuest = false;

//    void Start()
//    {
//        GetComponent<Collider2D>().isTrigger = true; // Bắt buộc phải là Trigger

//        if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
//    }

//    void Update()
//    {
//        if (isPlayerNear)
//        {
//            // 1. Nếu đang bật Hộp thoại chạy chữ -> Tắt cái chữ "Bấm Space" đi cho đỡ rối mắt
//            if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
//            {
//                if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
//            }
//            // 2. Nếu chưa nói chuyện hoặc đã nói xong -> Bật lại chữ "Bấm Space"
//            else
//            {
//                if (interactTooltipUI != null) interactTooltipUI.SetActive(true);

//                // 3. NẾU NHẤN SPACE -> BẮT ĐẦU NÓI CHUYỆN
//                if (Input.GetKeyDown(KeyCode.Space))
//                {
//                    if (talkSound != null) audioSource.PlayOneShot(talkSound);

//                    DialogueManager.instance.StartDialogue(lines); // Bắn câu thoại sang Manager
//                    hasReceivedQuest = true; // Mở khóa Cửa chuyển màn!
//                }
//            }
//        }
//    }

//    // Khi Tấm bước vào vùng của NPC
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player")) isPlayerNear = true;
//    }

//    // Khi Tấm bỏ đi xa
//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            isPlayerNear = false;
//            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
//        }
//    }
//}


using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NPC_Dialogue : MonoBehaviour
{
    [Header("Noi dung hop thoai")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Giao dien & am thanh")]
    public GameObject interactTooltipUI;
    public AudioClip talkSound;
    private AudioSource audioSource;

    private bool isPlayerNear = false;

    public static bool hasReceivedQuest = false;

    // ==== THÊM BIẾN NÀY ĐỂ CHỐNG DÍNH PHÍM ====
    private float interactCooldown = 0f;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;

        if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Trừ dần bộ đếm thời gian
        if (interactCooldown > 0) interactCooldown -= Time.deltaTime;

        if (isPlayerNear)
        {
            if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
            {
                if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

                // MẤU CHỐT Ở ĐÂY: Khi đang nói chuyện, giữ cooldown luôn ở mức 0.2 giây
                // Để khi hội thoại vừa kết thúc, nó phải đợi 0.2s sau mới cho bấm tiếp
                interactCooldown = 0.2f;
            }
            else
            {
                if (interactTooltipUI != null) interactTooltipUI.SetActive(true);

                // THÊM ĐIỀU KIỆN: Chỉ được nói chuyện khi Cooldown đã về 0
                if (interactCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
                {
                    if (talkSound != null) audioSource.PlayOneShot(talkSound);

                    DialogueManager.instance.StartDialogue(lines);
                    hasReceivedQuest = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNear = true;
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