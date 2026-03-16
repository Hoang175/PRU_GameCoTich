using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyVision : MonoBehaviour
{
    [Header("Cài đặt Tầm Nhìn")]
    public LayerMask obstacleLayer;
    public string gameOverScene = "Lv2_3_SanSau"; // Bị bắt ở sân sau thì load lại sân sau
    public AudioClip alertSound;
    public GameObject alertIconObject;

    [Header("Thoại khi bị bắt (Tiếng Anh)")]
    [TextArea(2, 4)]
    public string[] caughtLines = {
        "Stepmother: Aha! You little brat! How dare you sneak back here?",
        "Tam: Oh no... she caught me...",
        "System: You have been caught! Press [Space] to retry."
    };

    private SpriteRenderer parentSprite;
    private bool isCaught = false;

    void Start()
    {
        parentSprite = GetComponentInParent<SpriteRenderer>();
        if (alertIconObject != null) alertIconObject.SetActive(false);
    }

    void Update()
    {
        if (parentSprite != null)
        {
            float direction = parentSprite.flipX ? 1f : -1f;
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isCaught) return;

        if (collision.CompareTag("Player"))
        {
            Vector2 enemyPos = transform.parent.position;
            Vector2 playerPos = collision.transform.position;
            Vector2 directionToPlayer = playerPos - enemyPos;
            float distanceToPlayer = directionToPlayer.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(enemyPos, directionToPlayer, distanceToPlayer, obstacleLayer);

            if (hit.collider == null)
            {
                StartCoroutine(CatchPlayerRoutine());
            }
        }
    }

    IEnumerator CatchPlayerRoutine()
    {
        isCaught = true;

        // 1. TẮT ĐỘNG CƠ CỦA DÌ GHẺ/CÁM ĐỂ HỌ ĐỨNG IM!
        MonoBehaviour aiScript = transform.parent.GetComponent<DiGheAI>();
        if (aiScript != null) aiScript.enabled = false;

        // Trả về Animation đứng im (Idle)
        Animator anim = transform.parent.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("isRun", false);
            anim.SetBool("isMoveUp", false);
            anim.SetBool("isMoveDown", false);
        }

        // 2. Bật dấu [!] và tiếng động
        if (alertIconObject != null) alertIconObject.SetActive(true);
        if (alertSound != null) AudioSource.PlayClipAtPoint(alertSound, transform.position);

        // 3. Hiện thoại Tiếng Anh
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(caughtLines);
        }

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => DialogueManager.instance.isTalking == false);

        SceneManager.LoadScene(gameOverScene);
    }
}
