using UnityEngine;

public class Tam : MonoBehaviour
{
    [Header("Cài đặt Di chuyển")]
    public float moveSpeed = 5f;

    [Header("Cài đặt Lội Nước & Bước Chân")]
    public float waterSpeed = 2.5f;
    public AudioClip waterSound;       
    public AudioClip landStepSound;    
    public float stepInterval = 0.5f;

    [Header("Âm thanh (Audio)")]
    public AudioSource audioSource;    
    public AudioSource footstepSource; 

    private float originalSpeed;
    private bool isInWater = false;
    private float stepTimer = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 movement;
    private bool isPicking;

    private FacingDirection facing = FacingDirection.Down;
    private Vector3 baseSceneScale;
    private Vector2 baseSpriteSize;

    private enum FacingDirection { Left, Right, Up, Down }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        originalSpeed = moveSpeed;
        baseSceneScale = transform.localScale;

        if (spriteRenderer.sprite != null)
            baseSpriteSize = spriteRenderer.sprite.bounds.size;
    }

    void Update()
    {
        if (DialogueManager.instance != null && DialogueManager.instance.isTalking)
        {
            movement = Vector2.zero;
            animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);

            if (Input.GetKeyDown(KeyCode.Space)) DialogueManager.instance.DisplayNextSentence();

            if (footstepSource != null && footstepSource.isPlaying) footstepSource.Stop();
            return;
        }

        if (isPicking)
        {
            movement = Vector2.zero;
            animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartPick();
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0) moveY = 0;
        movement = new Vector2(moveX, moveY);

        animator.SetBool("moveSide", false); animator.SetBool("moveUp", false); animator.SetBool("moveDown", false);

        if (movement != Vector2.zero)
        {
            if (moveY > 0) { animator.SetBool("moveUp", true); facing = FacingDirection.Up; }
            else if (moveY < 0) { animator.SetBool("moveDown", true); facing = FacingDirection.Down; }
            else { animator.SetBool("moveSide", true); facing = moveX > 0 ? FacingDirection.Right : FacingDirection.Left; }

            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                if (footstepSource != null)
                {
                    if (isInWater && waterSound != null)
                    {
                        footstepSource.clip = waterSound;
                        footstepSource.Play();
                    }
                    else if (!isInWater && landStepSound != null)
                    {
                        footstepSource.clip = landStepSound;
                        footstepSource.Play();
                    }
                }
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
            if (footstepSource != null && footstepSource.isPlaying)
            {
                footstepSource.Stop(); 
            }
        }

        spriteRenderer.flipX = (facing == FacingDirection.Left);
    }

    void FixedUpdate()
    {
        if (!isPicking) rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void StartPick()
    {
        isPicking = true;
        movement = Vector2.zero;
        GetComponent<FishCollector>()?.TryInteract();

        switch (facing)
        {
            case FacingDirection.Left: case FacingDirection.Right: animator.SetTrigger("pickSide"); break;
            case FacingDirection.Up: animator.SetTrigger("pickUp"); break;
            case FacingDirection.Down: animator.SetTrigger("pickDown"); break;
        }
        Invoke("EndPick", 0.5f);
    }

    public void EndPick()
    {
        isPicking = false;
        animator.ResetTrigger("pickSide"); animator.ResetTrigger("pickUp"); animator.ResetTrigger("pickDown");
    }

    void LateUpdate()
    {
        if (spriteRenderer.sprite == null) return;
        Vector2 currentSize = spriteRenderer.sprite.bounds.size;
        if (currentSize.x == 0 || currentSize.y == 0) return;
        float scaleX = baseSpriteSize.x / currentSize.x;
        float scaleY = baseSpriteSize.y / currentSize.y;
        transform.localScale = new Vector3(baseSceneScale.x * scaleX, baseSceneScale.y * scaleY, baseSceneScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isInWater = true;
            moveSpeed = waterSpeed;
            if (audioSource != null && waterSound != null) audioSource.PlayOneShot(waterSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isInWater = false;
            moveSpeed = originalSpeed;
            if (footstepSource != null) footstepSource.Stop();
        }
    }
}