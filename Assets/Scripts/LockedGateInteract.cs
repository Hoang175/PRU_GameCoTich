using HE181245_DaoHuyHoang_UntoldTalesFairyTales;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
    [RequireComponent(typeof(Collider2D))]
    public class LockedGateInteract : MonoBehaviour
    {
        [Header("Cài đặt Cổng Khóa")]
        public string nextScene = "Lv2_3_SanSau";
        public AudioClip lockedSound;
        public AudioClip unlockSound;
        public float delayTime = 1f;

        public GameObject interactTooltipUI;
        public string tooltipText = "Press [Space] to unlock";

        private AudioSource audioSource;
        private bool isPlayerNear = false;
        private bool isTeleporting = false;
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

            if (isPlayerNear)
            {
                if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
                {
                    if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
                    interactCooldown = 0.2f;
                }
                else
                {
                    if (interactTooltipUI != null && !isTeleporting) interactTooltipUI.SetActive(true);

                    // Phím cứng PC
                    if (interactCooldown <= 0f && Input.GetKeyDown(KeyCode.Space) && !isTeleporting)
                    {
                        TriggerInteract();
                    }
                }
            }
        }

        // HÀM PUBLIC CHO NÚT BÀN TAY (MOBILE UI)
        public void TriggerInteract()
        {
            if (isPlayerNear && interactCooldown <= 0f && !isTeleporting)
            {
                TryOpenGate();
            }
        }

        void TryOpenGate()
        {
            // Kiểm tra chìa khóa từ class KeyItem cùng Namespace
            if (KeyItem.hasKey == false)
            {
                if (lockedSound != null) audioSource.PlayOneShot(lockedSound);
                string[] lines = { "Tam: The gate is locked tight. My Stepmother must have hidden the key somewhere around this yard..." };
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
            string[] lines = { "Tam: It's unlocked! I must sneak out to the well to check on little Bong!" };
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
}