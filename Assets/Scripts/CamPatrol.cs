using UnityEngine;

public class CamPatrol : MonoBehaviour
{
    [Header("Cài đặt Tuần tra")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Transform currentTarget;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Transform playerTransform;

    private Vector3 baseSceneScale;
    private Vector2 baseSpriteSize;

    void Start()
    {
        currentTarget = pointA;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        baseSceneScale = transform.localScale;
        if (spriteRenderer.sprite != null)
        {
            baseSpriteSize = spriteRenderer.sprite.bounds.size;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
        {
            if (animator != null) animator.SetBool("isMove", false);

            if (playerTransform != null)
            {
                // ĐÃ SỬA: Đảo ngược true/false để Cám quay đúng mặt vào Tấm
                if (playerTransform.position.x > transform.position.x)
                    spriteRenderer.flipX = false;
                else if (playerTransform.position.x < transform.position.x)
                    spriteRenderer.flipX = true;
            }

            return;
        }

        // =========================================================
        // CODE ĐI TUẦN
        // =========================================================
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        if (animator != null) animator.SetBool("isMove", true);

        // ĐÃ SỬA: Đảo ngược true/false để Cám không đi lùi nữa
        if (currentTarget.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (currentTarget.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (currentTarget == pointA) currentTarget = pointB;
            else currentTarget = pointA;
        }
    }

    // --- HÀM GIỮ ĐỒNG NHẤT KÍCH THƯỚC ---
    void LateUpdate()
    {
        if (spriteRenderer.sprite == null || baseSpriteSize.x == 0 || baseSpriteSize.y == 0) return;

        Vector2 currentSize = spriteRenderer.sprite.bounds.size;
        if (currentSize.x == 0 || currentSize.y == 0) return;

        float scaleX = baseSpriteSize.x / currentSize.x;
        float scaleY = baseSpriteSize.y / currentSize.y;

        transform.localScale = new Vector3(
            baseSceneScale.x * scaleX,
            baseSceneScale.y * scaleY,
            baseSceneScale.z
        );
    }
}