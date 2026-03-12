using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Cài đặt Spawner")]
    public float ghostDelay = 0.05f;
    private float ghostDelayTimer;

    [Header("Cài đặt Bóng")]
    public float ghostFadeSpeed = 4f; // Chỉnh lên 4 cho mờ nhanh, đẹp hơn
    public Color ghostColor = new Color(1f, 0.4f, 0.8f, 0.5f);

    private SpriteRenderer playerSR;
    private Vector3 lastPos;

    void Start()
    {
        playerSR = GetComponent<SpriteRenderer>();
        ghostDelayTimer = ghostDelay;
        lastPos = transform.position;
    }

    void Update()
    {
        if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
        {
            lastPos = transform.position;
            return;
        }

        // ĐÃ SỬA: Cho bộ đếm thời gian luôn chạy độc lập
        if (ghostDelayTimer > 0)
        {
            ghostDelayTimer -= Time.deltaTime;
        }

        // Khi đếm xong VÀ Tấm có nhúc nhích thì mới sinh bóng
        if (ghostDelayTimer <= 0 && Vector3.Distance(transform.position, lastPos) > 0.01f)
        {
            SpawnGhost();
            ghostDelayTimer = ghostDelay; 
        }

        lastPos = transform.position;
    }

    void SpawnGhost()
    {
        GameObject currentGhost = new GameObject("Bong_Mau_Hong");
        currentGhost.transform.position = transform.position;
        currentGhost.transform.rotation = transform.rotation;
        currentGhost.transform.localScale = transform.localScale;

        SpriteRenderer sr = currentGhost.AddComponent<SpriteRenderer>();
        sr.sprite = playerSR.sprite;
        sr.color = ghostColor;

        // ==== ĐÂY LÀ DÒNG CHỮA BỆNH TÀNG HÌNH ====
        sr.material = playerSR.material; // Bắt buộc phải copy material của Tấm sang

        sr.sortingLayerName = playerSR.sortingLayerName;
        sr.sortingOrder = playerSR.sortingOrder - 1;
        sr.flipX = playerSR.flipX;

        SpriteGhost ghostScript = currentGhost.AddComponent<SpriteGhost>();
        ghostScript.fadeSpeed = ghostFadeSpeed;
    }
}