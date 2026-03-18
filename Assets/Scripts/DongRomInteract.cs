using UnityEngine;
using TMPro;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
    [RequireComponent(typeof(Collider2D))]
    public class DongRomInteract : MonoBehaviour
    {
        [Header("Cài đặt Đống Rơm")]
        public AudioClip searchSound;
        public GameObject keyObject; 
        public GameObject interactTooltipUI;
        public string tooltipText = "Press [Space] to search";

        private AudioSource audioSource;
        private bool isPlayerNear = false;
        private bool isSearched = false;

        private float interactCooldown = 0f;

        void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

            // Giấu chìa khóa đi khi mới vào màn
            if (keyObject != null) keyObject.SetActive(false);
        }

        void Update()
        {
            if (interactCooldown > 0) interactCooldown -= Time.deltaTime;

            if (isPlayerNear && !isSearched)
            {
                if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
                {
                    if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
                    interactCooldown = 0.2f;
                }
                else
                {
                    if (interactTooltipUI != null) interactTooltipUI.SetActive(true);

                    // Phím cứng PC
                    if (interactCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
                    {
                        TriggerInteract();
                    }
                }
            }
        }

        // HÀM PUBLIC CHO NÚT BÀN TAY (MOBILE UI)
        public void TriggerInteract()
        {
            if (isPlayerNear && !isSearched && interactCooldown <= 0f)
            {
                isSearched = true;
                if (searchSound != null) audioSource.PlayOneShot(searchSound);
                if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

                // Hiện chìa khóa ra trên map
                if (keyObject != null) keyObject.SetActive(true);

                string[] lines = { "Tam: The straw is so thick... Ah! A key dropped out!" };
                if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerNear = true;
                if (!isSearched && interactTooltipUI != null)
                {
                    interactTooltipUI.GetComponentInChildren<TextMeshProUGUI>().text = tooltipText;
                    interactTooltipUI.SetActive(true);
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
}