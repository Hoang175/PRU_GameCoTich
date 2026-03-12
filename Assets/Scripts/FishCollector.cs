
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    public GameObject interactTooltipUI;

    [Header("Phần thưởng (Reward)")]
    public GameObject awardPrefab;

    [Header("Âm thanh (Audio)")]
    public AudioSource sfxSource;
    public AudioClip pickFishSound;
    public AudioClip talkToCamSound;
    public AudioClip questCompleteSound;

    [Header("Chuyển cảnh (Next Scene)")]
    public string nextSceneName = "Lv2_1_DuongVao";
    public float timeToWait = 5f;

    private GameObject currentFishNearMe;
    private bool isNearCam = false;

    void Start()
    {
        if (fishUIContainer != null) fishUIContainer.SetActive(false);
        if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
        if (sfxSource == null) sfxSource = GetComponent<AudioSource>();
    }

    public void TryInteract()
    {
        if (isNearCam)
        {
            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
            PlaySound(talkToCamSound);

            if (!isQuestStarted)
            {
                isQuestStarted = true;
                if (fishUIContainer != null) { fishUIContainer.SetActive(true); UpdateUIText(); }

                string[] lines = {
                    "Cám: Chị Tấm ơi, dì bảo hai chị em mình ra ao bắt tôm bắt tép.",
                    "Cám: Ai bắt được đầy giỏ trước sẽ được dì thưởng cho một cái yếm đỏ đấy!",
                    "Tấm: Thật thế hả Cám? Vậy chị em mình cùng thi nhé!",
                    "Cám: Chị mau lội xuống ao bắt đủ 6 con cá bống đi, em chờ trên bờ cho."
                };
                DialogueManager.instance.StartDialogue(lines);
            }
            else if (isQuestStarted && fishCount < targetFish)
            {
                string[] lines = {
                    "Cám: Chị Tấm lười thế, mới bắt được " + fishCount + " con thôi à? Nhanh tay lên kẻo tối!",
                    "Tấm: Chị đang cố đây, cá bống hôm nay trốn kỹ quá."
                };
                DialogueManager.instance.StartDialogue(lines);
            }
            else if (isQuestStarted && fishCount >= targetFish && !isRewardClaimed)
            {
                isRewardClaimed = true;
                if (fishUIContainer != null) fishUIContainer.SetActive(false);

                string[] lines = {
                    "Tấm: Cám ơi, chị bắt đủ 6 con cá bống rồi này!",
                    "Cám: Chị Tấm ơi chị Tấm! Đầu chị lấm, chị hụp cho sâu kẻo về dì mắng.",
                    "Tấm: Ủa vậy à, để chị xuống ao gội đầu chút đã nhé.",
                    "Cám: Đưa giỏ cá đây em giữ hộ cho...",
                    "Hệ thống: (Cám nhanh tay trút sạch giỏ cá bống của Tấm sang giỏ mình!)"
                };
                DialogueManager.instance.StartDialogue(lines);

                PlaySound(questCompleteSound);
                GiveReward();
            }
            else if (isRewardClaimed)
            {
                string[] lines = {
                    "Cám: Chị cứ tắm mát đi nhé, em mang cá về nhận yếm đỏ trước đây! Hahaha."
                };
                DialogueManager.instance.StartDialogue(lines);
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
        if (awardPrefab != null)
        {
            GameObject reward = Instantiate(awardPrefab, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
            Destroy(reward, timeToWait);
            Debug.Log(">>> [Hệ thống]: Phần thưởng rớt ra và sẽ biến mất sau 5 giây!");
        }
        Invoke("LoadNextScene", timeToWait);
    }

    void LoadNextScene()
    {
        Debug.Log(">>> [Hệ thống]: Đang chuyển sang màn " + nextSceneName);
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Chưa nhập tên Scene tiếp theo trong Inspector!");
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish")) currentFishNearMe = collision.gameObject;
        else if (collision.CompareTag("NPC"))
        {
            isNearCam = true;
            if (interactTooltipUI != null && DialogueManager.instance != null && !DialogueManager.instance.isTalking)
            {
                interactTooltipUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish")) currentFishNearMe = null;
        else if (collision.CompareTag("NPC"))
        {
            isNearCam = false;
            if (interactTooltipUI != null)
            {
                interactTooltipUI.SetActive(false);
            }
        }
    }
}