using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NguaJump : MonoBehaviour
{
    public float lucNhay = 15f;
    public AudioClip tiengNguaHi; // Bỏ âm thanh ngựa hí vào đây
    public AudioClip tiengDatChan; // Tiếng cộp cộp khi rớt xuống đất

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isGrounded = true; // Kiểm tra xem ngựa đang ở trên đất hay trên không

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    //void Update()
    //{
    //    // CHỈ CHO PHÉP NHẢY KHI GAME ĐANG CHẠY (Đã xong Intro) VÀ NGỰA ĐANG ĐỨNG TRÊN ĐẤT
    //    if (Level4Director.isPlaying && isGrounded && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Jump();
    //    }
    //}
    void Update()
    {
        // ĐÃ ĐỔI PHÍM NHẢY THÀNH MŨI TÊN LÊN HOẶC PHÍM W
        if (Level4Director.isPlaying && isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            Jump();
        }
    }

    void Jump()
    {
        isGrounded = false;

        // Reset vận tốc Y trước khi nhảy để tránh lỗi nhảy dồn
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        // Đẩy con ngựa lên trời!
        rb.AddForce(Vector2.up * lucNhay, ForceMode2D.Impulse);

        if (tiengNguaHi != null) audioSource.PlayOneShot(tiengNguaHi);
    }

    // ==========================================
    // KIỂM TRA CHẠM ĐẤT ĐỂ CHO NHẢY TIẾP
    // ==========================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Khi rơi xuống đụng trúng cái sàn vô hình (Nhớ gắn tag "Ground" cho cái InvisibleGround)
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded) // Nếu trước đó đang ở trên không
            {
                isGrounded = true;
                if (tiengDatChan != null) audioSource.PlayOneShot(tiengDatChan);
            }
        }
    }
}