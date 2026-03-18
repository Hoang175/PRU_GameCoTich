using UnityEngine;
using TMPro;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
    [RequireComponent(typeof(Collider2D))]
    public class KeyItem : MonoBehaviour
    {
        public AudioClip pickupSound;
        public GameObject interactTooltipUI;
        public string tooltipText = "Press [Space] to pick up key";

        private AudioSource audioSource;
        private bool isPlayerNear = false;
        private float interactCooldown = 0f;

        public static bool hasKey = false;

        void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            hasKey = false;
        }

        void Update()
        {
            if (interactCooldown > 0) interactCooldown -= Time.deltaTime;

            if (isPlayerNear)
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
            if (isPlayerNear && interactCooldown <= 0f)
            {
                hasKey = true; // Nhận chìa khóa
                if (pickupSound != null) audioSource.PlayOneShot(pickupSound);
                if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

                string[] lines = { "Tam: This key is rusty, I hope it can open the gate." };
                if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);

                gameObject.SetActive(false); // Biến mất sau khi nhặt
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerNear = true;
                if (interactTooltipUI != null)
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