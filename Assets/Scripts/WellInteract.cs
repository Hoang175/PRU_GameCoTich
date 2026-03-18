using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
    [RequireComponent(typeof(Collider2D))]
    public class WellInteract : MonoBehaviour
    {
        public GameObject interactTooltipUI;
        public string tooltipText = "Press [Space] to look into the well";

        [Header("Cài đặt Video Chuyển Cảnh")]
        public VideoPlayer videoPlayer;
        public string nextScene = "Lv3_1_PhepMau";

        private bool isPlayerNear = false;
        private bool isDone = false;
        private float interactCooldown = 0f;

        void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
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
                    if (interactTooltipUI != null && !isDone) interactTooltipUI.SetActive(true);

                    if (interactCooldown <= 0f && !isDone && Input.GetKeyDown(KeyCode.Space))
                    {
                        TriggerInteract();
                    }
                }
            }
        }

        public void TriggerInteract()
        {
            if (isPlayerNear && !isDone && interactCooldown <= 0f)
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
                if (interactTooltipUI != null)
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