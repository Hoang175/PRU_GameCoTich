using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class DongRomInteract : MonoBehaviour
{
    [Header("Cài đặt Đống Rơm")]
    public AudioClip searchSound;
    public GameObject keyObject; // KÉO OBJECT CHÌA KHÓA TRÊN MAP VÀO ĐÂY
    public GameObject interactTooltipUI;
    public string tooltipText = "Nhấn [Space] để bới rơm";

    private AudioSource audioSource;
    private bool isPlayerNear = false;
    private bool isSearched = false;

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
        if (isPlayerNear && !isSearched && Input.GetKeyDown(KeyCode.Space))
        {
            isSearched = true;
            if (searchSound != null) audioSource.PlayOneShot(searchSound);
            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

            // HIỆN CÁI CHÌA KHÓA RA TRÊN BẢN ĐỒ!
            if (keyObject != null) keyObject.SetActive(true);

            string[] lines = { "Tấm: Rơm rậm quá... A! Có cái chìa khóa rớt ra kìa!" };
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


//using UnityEngine;
//using TMPro; // THÊM DÒNG NÀY ĐỂ CHỈNH CHỮ

//[RequireComponent(typeof(Collider2D))]
//public class DongRomInteract : MonoBehaviour
//{
//    [Header("Cài đặt Đống Rơm")]
//    public AudioClip searchSound;
//    public GameObject keyIcon;
//    public GameObject interactTooltipUI;

//    // THÊM CÁI NÀY: Chữ bạn muốn hiển thị
//    public string tooltipText = "Nhấn [Space] để bới rơm";

//    private AudioSource audioSource;
//    private bool isPlayerNear = false;
//    private bool isSearched = false;

//    public static bool hasKey = false;

//    void Start()
//    {
//        GetComponent<Collider2D>().isTrigger = true;
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
//        hasKey = false;
//    }

//    void Update()
//    {
//        if (isPlayerNear && !isSearched && Input.GetKeyDown(KeyCode.Space))
//        {
//            SearchStraw();
//        }
//    }

//    void SearchStraw()
//    {
//        isSearched = true;
//        hasKey = true;
//        if (searchSound != null) audioSource.PlayOneShot(searchSound);
//        if (keyIcon != null) keyIcon.SetActive(false);

//        // Đã bới xong thì TẮT CÁI TOOLTIP ĐI cho đỡ vướng mắt
//        if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

//        string[] lines = { "Tấm: A! Đây rồi, một chiếc chìa khóa gỉ sét giấu kỹ dưới đống rơm." };
//        if (DialogueManager.instance != null && !DialogueManager.instance.isTalking)
//        {
//            DialogueManager.instance.StartDialogue(lines);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            isPlayerNear = true;
//            if (!isSearched && interactTooltipUI != null)
//            {
//                // MA THUẬT NẰM Ở ĐÂY: Lấy cái chữ ra và đổi nội dung!
//                interactTooltipUI.GetComponentInChildren<TextMeshProUGUI>().text = tooltipText;
//                interactTooltipUI.SetActive(true);
//            }
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            isPlayerNear = false;
//            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);
//        }
//    }
//}