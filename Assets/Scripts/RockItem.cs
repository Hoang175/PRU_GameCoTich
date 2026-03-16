using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class RockItem : MonoBehaviour
{
    public AudioClip pickupSound;
    public GameObject interactTooltipUI;
    public string tooltipText = "Nhấn [Space] để nhặt sỏi";

    private AudioSource audioSource;
    private bool isPlayerNear = false;

    // Biến toàn cục báo cho hệ thống biết Tấm đã có sỏi
    public static bool hasRock = false;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        hasRock = false; // Reset mỗi lần vào màn
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Space))
        {
            hasRock = true;
            //if (pickupSound != null) audioSource.PlayOneShot(pickupSound);

            if (pickupSound != null)
            {
                // ==== DÙNG LỆNH NÀY THAY THẾ ====
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

            //string[] lines = { "Tấm: Một nắm sỏi... Có thể dùng nó để gây tiếng động đánh lạc hướng Dì!" };
            string[] lines = { "Tam: A handful of pebbles... I can use these to make a noise and distract her!" };
            if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);

            gameObject.SetActive(false); // Ẩn viên sỏi đi
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