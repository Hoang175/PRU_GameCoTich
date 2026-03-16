using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class DistractTarget : MonoBehaviour
{
    public AudioClip clangSound; // Tiếng Keng! vang lên
    public GameObject interactTooltipUI;
    public string tooltipText = "Press [Space] to throw pebbles";

    private AudioSource audioSource;
    private bool isPlayerNear = false;
    private bool isThrown = false; // Chỉ cho ném 1 lần

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // NẾU: Tấm ở gần + Chưa ném + ĐÃ NHẶT SỎI + Bấm Space
        if (isPlayerNear && !isThrown && RockItem.hasRock && Input.GetKeyDown(KeyCode.Space))
        {
            ThrowRock();
        }
    }

    void ThrowRock()
    {
        isThrown = true;
        RockItem.hasRock = false; // Ném xong thì mất sỏi

        if (clangSound != null) audioSource.PlayOneShot(clangSound);
        if (interactTooltipUI != null) interactTooltipUI.SetActive(false);

        // Hiển thị thoại Tiếng Anh
        string[] lines = {
            "Tam: *Throws pebble*",
            "System: *CLANG!*",
            "Stepmother: Who is sneaking around my yard?! I'll break your legs!"
        };

        if (DialogueManager.instance != null) DialogueManager.instance.StartDialogue(lines);

        // GỬI TÍN HIỆU CHO DÌ GHẺ BIẾT CÓ TIẾNG ĐỘNG Ở ĐÂY!
        // (Chúng ta sẽ update não DiGheAI ở bước tiếp theo để bả nhận tín hiệu này)
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