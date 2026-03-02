using UnityEngine;
using TMPro;

public class FishCollector : MonoBehaviour
{
    [Header("Cài đặt Nhiệm vụ")]
    public int fishCount = 0;
    public int targetFish = 6;
    public bool isQuestStarted = false;
    public bool isRewardClaimed = false;

    [Header("UI Giao diện")]
    public TextMeshProUGUI fishCounterText;
    public GameObject fishUIContainer;

    [Header("Âm thanh (Audio)")]
    public AudioSource sfxSource;           // Nguồn phát âm thanh (Cái loa gắn trên Tấm)
    public AudioClip pickFishSound;         // File âm thanh nhặt cá
    public AudioClip talkToCamSound;        // File âm thanh Ting! khi nói chuyện
    public AudioClip questCompleteSound;    // File âm thanh khi trả nhiệm vụ xong

    private GameObject currentFishNearMe;
    private bool isNearCam = false;

    void Start()
    {
        if (fishUIContainer != null) fishUIContainer.SetActive(false);

        // Tự động tìm AudioSource trên người Tấm nếu bạn quên kéo thả
        if (sfxSource == null) sfxSource = GetComponent<AudioSource>();
    }

    public void TryInteract()
    {
        if (isNearCam)
        {
            // Phát tiếng "Ting" mỗi khi nói chuyện với Cám
            PlaySound(talkToCamSound);

            if (!isQuestStarted)
            {
                isQuestStarted = true;
                if (fishUIContainer != null)
                {
                    fishUIContainer.SetActive(true);
                    UpdateUIText();
                }
                Debug.Log("Cám: Chị Tấm đi bắt 6 con cá bống đi!");
            }
            else if (isQuestStarted && fishCount < targetFish)
            {
                Debug.Log("Cám: Mới bắt được " + fishCount + " con thôi à? Nhanh lên!");
            }
            else if (isQuestStarted && fishCount >= targetFish && !isRewardClaimed)
            {
                isRewardClaimed = true;
                if (fishUIContainer != null) fishUIContainer.SetActive(false);

                // Phát tiếng nhận thưởng
                PlaySound(questCompleteSound);
                Debug.Log("Cám: Đưa giỏ cá đây cho em...");
                GiveReward();
            }
            else if (isRewardClaimed)
            {
                Debug.Log("Cám: Yếm Đỏ sẽ là của mình hahaha!");
            }
            return;
        }

        if (currentFishNearMe != null)
        {
            if (isQuestStarted && !isRewardClaimed && fishCount < targetFish)
            {
                AddFish();
                Destroy(currentFishNearMe);
                currentFishNearMe = null;
            }
        }
    }

    public void AddFish()
    {
        fishCount++;
        UpdateUIText();

        // Phát tiếng nhặt cá
        PlaySound(pickFishSound);
        Debug.Log("Tấm: Bắt được con thứ " + fishCount + " rồi!");
    }

    private void UpdateUIText()
    {
        if (fishCounterText != null) fishCounterText.text = fishCount + " / " + targetFish;
    }

    void GiveReward()
    {
        Debug.Log(">>> [Hệ thống]: Bạn đã bị Cám trút sạch giỏ cá!");
    }

    // Hàm hỗ trợ phát âm thanh để code gọn hơn
    private void PlaySound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            // PlayOneShot giúp các âm thanh có thể đè lên nhau (ví dụ nhặt 2 con cá liên tục) mà không bị ngắt quãng
            sfxSource.PlayOneShot(clip);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish")) currentFishNearMe = collision.gameObject;
        else if (collision.CompareTag("NPC")) isNearCam = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish")) currentFishNearMe = null;
        else if (collision.CompareTag("NPC")) isNearCam = false;
    }
}


//using UnityEngine;
//using TMPro;

//public class FishCollector : MonoBehaviour
//{
//    [Header("Cài đặt Nhiệm vụ")]
//    public int fishCount = 0;
//    public int targetFish = 6;

//    // Các biến trạng thái của nhiệm vụ
//    public bool isQuestStarted = false;
//    public bool isRewardClaimed = false;

//    [Header("UI Giao diện")]
//    public TextMeshProUGUI fishCounterText;
//    public GameObject fishUIContainer;

//    private GameObject currentFishNearMe;
//    private bool isNearCam = false;

//    public GameObject rewardPrefab;

//    void Start()
//    {
//        if (fishUIContainer != null)
//        {
//            fishUIContainer.SetActive(false);
//            Debug.Log(">>> [Hệ thống]: UI đã ẩn, chờ Cám giao việc.");
//        }
//    }

//    public void TryInteract()
//    {
//        // ==========================================
//        // TRƯỜNG HỢP 1: ĐỨNG GẦN NPC CÁM
//        // ==========================================
//        if (isNearCam)
//        {
//            // 1a. Chưa nhận nhiệm vụ
//            if (!isQuestStarted)
//            {
//                isQuestStarted = true;
//                if (fishUIContainer != null)
//                {
//                    fishUIContainer.SetActive(true);
//                    UpdateUIText();
//                }
//                Debug.Log("Cám: Chị Tấm ơi! Dì bảo ai bắt được nhiều cá thì được thưởng Yếm Đỏ. Chị đi bắt 6 con cá bống đi!");
//            }
//            // 1b. Đã nhận nhưng CHƯA đủ cá
//            else if (isQuestStarted && fishCount < targetFish)
//            {
//                Debug.Log("Cám: Chị Tấm lười thế, mới bắt được " + fishCount + " con thôi à? Nhanh lên!");
//            }
//            // 1c. Đã đủ cá và CHƯA nhận thưởng (TRẢ NHIỆM VỤ)
//            else if (isQuestStarted && fishCount >= targetFish && !isRewardClaimed)
//            {
//                isRewardClaimed = true;
//                if (fishUIContainer != null)
//                {
//                    fishUIContainer.SetActive(false); // Ẩn UI đi
//                }

//                Debug.Log("Cám: Chị Tấm ơi chị Tấm, đầu chị lấm, chị hụp cho sâu kẻo về dì mắng...");
//                GiveReward(); // Gọi hàm trao phần thưởng
//            }
//            // 1d. Đã trả nhiệm vụ rồi, bấm nói chuyện tiếp
//            else if (isRewardClaimed)
//            {
//                Debug.Log("Cám (cười thầm): Giỏ cá này là của mình rồi, Yếm Đỏ sẽ là của mình hahaha!");
//            }

//            return; // Quan trọng: Đã tương tác với Cám thì dừng lại, không xét việc nhặt cá bên dưới nữa
//        }

//        // ==========================================
//        // TRƯỜNG HỢP 2: ĐỨNG GẦN CON CÁ
//        // ==========================================
//        if (currentFishNearMe != null)
//        {
//            if (isQuestStarted && !isRewardClaimed && fishCount < targetFish)
//            {
//                AddFish();
//                Destroy(currentFishNearMe);
//                currentFishNearMe = null;
//            }
//            else if (!isQuestStarted)
//            {
//                Debug.Log("Tấm: Phải ra hỏi Cám xem dì ghẻ dặn gì đã rồi mới bắt cá.");
//            }
//            else if (fishCount >= targetFish)
//            {
//                Debug.Log("Tấm: Giỏ đầy rồi, mau mang về cho Cám thôi!");
//            }
//        }
//    }

//    public void AddFish()
//    {
//        fishCount++;
//        UpdateUIText();
//        Debug.Log("Tấm: Bắt được con thứ " + fishCount + " rồi!");
//    }

//    private void UpdateUIText()
//    {
//        if (fishCounterText != null)
//        {
//            fishCounterText.text = fishCount + " / " + targetFish;
//        }
//    }

//    // ==========================================
//    // HÀM XỬ LÝ TRAO PHẦN THƯỞNG
//    // ==========================================
//    void GiveReward()
//    {
//        Debug.Log(">>> [Hệ thống]: Bạn đã bị Cám trút sạch giỏ cá!");
//        Debug.Log(">>> [Hệ thống]: Phần thưởng an ủi: Nhận được [1x Yếm Đỏ (Trong mơ)] và [1x Con Cá Bống còn sót lại]");

//        // GỢI Ý: NẾU BẠN CÓ PREFAB VẬT PHẨM RƠI RA ĐẤT (ví dụ cái Yếm Đỏ)
//        // Bạn có thể dùng lệnh Instantiate ở đây để tạo vật phẩm rơi ra trước mặt Tấm
//         GameObject yEmDo = Instantiate(rewardPrefab, transform.position + Vector3.up, Quaternion.identity);
//    }

//    // Các hàm Trigger giữ nguyên
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Fish")) currentFishNearMe = collision.gameObject;
//        else if (collision.CompareTag("NPC")) isNearCam = true;
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Fish")) currentFishNearMe = null;
//        else if (collision.CompareTag("NPC")) isNearCam = false;
//    }
//}