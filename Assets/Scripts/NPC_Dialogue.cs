using UnityEngine;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
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

                    // THÊM ĐIỀU KIỆN: Chỉ được nói chuyện khi Cooldown đã về 0 (Dùng cho PC)
                    if (interactCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
                    {
                        TriggerDialogue(); // Gọi hàm public thay vì viết trực tiếp ở đây
                    }
                }
            }
        }

        // TẠO HÀM PUBLIC NÀY ĐỂ NÚT BÀN TAY BÊN NGOÀI CÓ THỂ GỌI VÀO ĐƯỢC
        public void TriggerDialogue()
        {
            // Kiểm tra xem người chơi có đang ở gần không và chưa nói chuyện
            if (isPlayerNear && interactCooldown <= 0f && DialogueManager.instance != null && !DialogueManager.instance.isTalking)
            {
                if (talkSound != null) audioSource.PlayOneShot(talkSound);

                DialogueManager.instance.StartDialogue(lines);
                hasReceivedQuest = true;
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
}