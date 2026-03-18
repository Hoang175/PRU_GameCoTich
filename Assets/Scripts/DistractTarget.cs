using UnityEngine;
using TMPro;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
    [RequireComponent(typeof(Collider2D))]
    public class DistractTarget : MonoBehaviour
    {
        public AudioClip clangSound;
        public GameObject interactTooltipUI;
        public string tooltipText = "Press [Space] to throw pebbles";

        private AudioSource audioSource;
        private bool isPlayerNear = false;
        private bool isThrown = false;
        private float interactCooldown = 0f;

        void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        }

        void Update()
        {
            if (interactCooldown > 0) interactCooldown -= Time.deltaTime;

            if (isPlayerNear && !isThrown && RockItem.hasRock)
            {
                if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
                {
                    if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
                    interactCooldown = 0.2f;
                }
                else
                {
                    if (interactTooltipUI != null) interactTooltipUI.SetActive(true);

                    // PC
                    if (interactCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
                    {
                        TriggerInteract();
                    }
                }
            }
        }

        public void TriggerInteract()
        {
            if (isPlayerNear && !isThrown && RockItem.hasRock && interactCooldown <= 0f)
            {
                ThrowRock();
            }
        }

        void ThrowRock()
        {
            isThrown = true;
            RockItem.hasRock = false;

            if (clangSound != null) audioSource.PlayOneShot(clangSound);
            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

            string[] lines = {
                "Tam: *Throws pebble*",
                "System: *CLANG!*",
                "Stepmother: Who is sneaking around my yard?! I'll break your legs!"
            };

            if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);

            DiGheAI aiScript = FindAnyObjectByType<DiGheAI>();
            if (aiScript != null)
            {
                aiScript.InvestigateSound(transform.position);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isThrown && RockItem.hasRock)
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