//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class TeleportDoor : MonoBehaviour
//{
//    [Header("Change Screens")]
//    public string sceneToLoad = "Lv2_2_SanTruoc"; // Tên màn tiếp theo
//    public AudioClip transitionSound;             // Nhạc/Tiếng khi chuyển màn
//    public float delayTime = 1f;                  // Chờ 1 giây để nghe hết tiếng rồi mới sang map

//    private AudioSource audioSource;
//    private bool isTeleporting = false; // Ngăn chặn việc kẹt lệnh nếu lỡ chạm nhiều lần

//    void Start()
//    {
//        // Tự động tạo một cái Loa cho Cánh cửa nếu bạn chưa gắn
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        // Khi Tấm chạm vào Cửa
//        // Thêm dòng này để kiểm tra xem có cái gì chạm vào cửa không
//        Debug.Log("Có vật chạm vào cửa: " + collision.gameObject.name);

//        if (collision.CompareTag("Player") && !isTeleporting)
//        {
//            StartCoroutine(TeleportRoutine());
//        }
//    }

//    // Hàm đếm ngược
//    IEnumerator TeleportRoutine()
//    {
//        isTeleporting = true;

//        // 1. Phát âm thanh
//        if (transitionSound != null)
//        {
//            audioSource.PlayOneShot(transitionSound);
//        }

//        // 2. Chờ một chút cho âm thanh kêu xong
//        yield return new WaitForSeconds(delayTime);

//        // 3. Chuyển scene
//        SceneManager.LoadScene(sceneToLoad);
//    }
//}

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