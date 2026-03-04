using UnityEngine;

public class Tam : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 movement;
    private bool isPicking;

    private FacingDirection facing = FacingDirection.Down;

    private Vector3 baseSceneScale;
    private Vector2 baseSpriteSize;

    private enum FacingDirection
    {
        Left, Right, Up, Down
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        baseSceneScale = transform.localScale;

        if (spriteRenderer.sprite != null)
            baseSpriteSize = spriteRenderer.sprite.bounds.size;
    }

    void Update()
    {

        // ==== THÊM ĐOẠN NÀY LÊN ĐẦU TIÊN CỦA UPDATE ====
        if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
        {
            movement = Vector2.zero;
            animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);

            // Nếu đang nói chuyện mà bấm Space -> Bỏ qua câu tiếp theo
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DialogueManager.instance.DisplayNextSentence();
            }
            return; // Dừng luôn Update, không cho chạy xuống dưới
        }


        // Nếu đang nhặt đồ/tương tác thì không cho di chuyển
        if (isPicking)
        {
            movement = Vector2.zero;
            animator.SetBool("moveSide", false);
            animator.SetBool("moveUp", false);
            animator.SetBool("moveDown", false);
            return;
        }

        // Nhấn Space để Tương tác (Nói chuyện với Cám hoặc Nhặt cá)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartPick();
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0) moveY = 0; // Ưu tiên đi ngang nếu bấm chéo
        movement = new Vector2(moveX, moveY);

        animator.SetBool("moveSide", false);
        animator.SetBool("moveUp", false);
        animator.SetBool("moveDown", false);

        if (movement != Vector2.zero)
        {
            if (moveY > 0)
            {
                animator.SetBool("moveUp", true);
                facing = FacingDirection.Up;
            }
            else if (moveY < 0)
            {
                animator.SetBool("moveDown", true);
                facing = FacingDirection.Down;
            }
            else
            {
                animator.SetBool("moveSide", true);
                facing = moveX > 0 ? FacingDirection.Right : FacingDirection.Left;
            }
        }

        spriteRenderer.flipX = (facing == FacingDirection.Left);
    }

    void FixedUpdate()
    {
        if (!isPicking)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // ===== PICK / INTERACT LOGIC =====
    void StartPick()
    {
        isPicking = true;
        movement = Vector2.zero;

        GetComponent<FishCollector>()?.TryInteract();

        switch (facing)
        {
            case FacingDirection.Left:
            case FacingDirection.Right:
                animator.SetTrigger("pickSide");
                break;
            case FacingDirection.Up:
                animator.SetTrigger("pickUp");
                break;
            case FacingDirection.Down:
                animator.SetTrigger("pickDown");
                break;
        }

        Invoke("EndPick", 0.5f);
    }

    public void EndPick()
    {
        isPicking = false;
        animator.ResetTrigger("pickSide");
        animator.ResetTrigger("pickUp");
        animator.ResetTrigger("pickDown");
    }

    void LateUpdate()
    {
        if (spriteRenderer.sprite == null) return;

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