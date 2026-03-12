using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class KeyItem : MonoBehaviour
{
    public AudioClip pickupSound; // Tiếng nhặt đồ keng keng
    public GameObject interactTooltipUI; // Kéo GameHUD/InteractTooltip vào đây
    public string tooltipText = "Nhấn [Space] để nhặt chìa";

    private AudioSource audioSource;
    private bool isPlayerNear = false;

    // DỜI BIẾN HẢI QUAN hasKey SANG ĐÂY
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
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Space))
        {
            hasKey = true; // CHÍNH THỨC SỞ HỮU CHÌA KHÓA
            if (pickupSound != null) audioSource.PlayOneShot(pickupSound);
            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

            string[] lines = { "Tấm: Chìa khóa gỉ sét thế này, hy vọng mở được cổng." };
            if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);

            gameObject.SetActive(false); // Nhặt xong thì chìa biến mất
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