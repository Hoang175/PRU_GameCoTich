using HE181245_DaoHuyHoang_UntoldTalesFairyTales;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportDoor : MonoBehaviour
{
    [Header("Cài đặt Chuyển Cảnh")]
    public string sceneToLoad = "Lv2_2_SanTruoc";
    public AudioClip transitionSound;
    public float delayTime = 1f;

    private AudioSource audioSource;
    private bool isTeleporting = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTeleporting)
        {
            // ==== PHẦN MỚI: KIỂM TRA ĐÃ NHẬN NHIỆM VỤ CHƯA ====
            if (NPC_Dialogue.hasReceivedQuest == false)
            {
                Debug.Log("Chưa hỏi thăm Bác Nông Dân! Cửa đóng kín mít.");
                // Tùy chọn: Bạn có thể cho Tấm tự độc thoại ở đây
                string[] lines = { "Tấm: Mình phải hỏi thăm Bác Nông Dân xem tình hình ở nhà thế nào đã, không thể vào liều được." };
                if (DialogueManager.instance != null && !DialogueManager.instance.isTalking)
                {
                    DialogueManager.instance.StartDialogue(lines);
                }
                return; // Lệnh này đá văng code ra ngoài, ngăn không cho chuyển cảnh!
            }

            // NẾU ĐÃ NÓI CHUYỆN RỒI -> CHO CHUYỂN MÀN
            StartCoroutine(TeleportRoutine());
        }
    }

    IEnumerator TeleportRoutine()
    {
        isTeleporting = true;

        if (transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }

        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(sceneToLoad);
    }
}