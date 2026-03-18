using UnityEngine;
using TMPro;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
    [RequireComponent(typeof(Collider2D))]
    public class RockItem : MonoBehaviour
    {
        public AudioClip pickupSound;
        public GameObject interactTooltipUI;
        public string tooltipText = "Press [Space] to pick up pebbles";

        private AudioSource audioSource;
        private bool isPlayerNear = false;
        private float interactCooldown = 0f;

        public static bool hasRock = false;

        void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            hasRock = false;
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

                    // Bấm bằng PC
                    if (interactCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
                    {
                        TriggerInteract();
                    }
                }
            }
        }

        // DÙNG CHO NÚT BÀN TAY BẤM VÀO
        public void TriggerInteract()
        {
            if (isPlayerNear && interactCooldown <= 0f)
            {
                hasRock = true;
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

                string[] lines = { "Tam: A handful of pebbles... I can use these to make a noise and distract her!" };
                if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);

                gameObject.SetActive(false);
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