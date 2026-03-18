//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(Collider2D))]
//public class NguaJump : MonoBehaviour
//{
//    public float lucNhay = 15f;
//    public AudioClip tiengNguaHi; // Bỏ âm thanh ngựa hí vào đây
//    public AudioClip tiengDatChan; // Tiếng cộp cộp khi rớt xuống đất

//    private Rigidbody2D rb;
//    private AudioSource audioSource;
//    private bool isGrounded = true; // Kiểm tra xem ngựa đang ở trên đất hay trên không

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        audioSource = gameObject.AddComponent<AudioSource>();
//    }

//    //void Update()
//    //{
//    //    // CHỈ CHO PHÉP NHẢY KHI GAME ĐANG CHẠY (Đã xong Intro) VÀ NGỰA ĐANG ĐỨNG TRÊN ĐẤT
//    //    if (Level4Director.isPlaying && isGrounded && Input.GetKeyDown(KeyCode.Space))
//    //    {
//    //        Jump();
//    //    }
//    //}
//    void Update()
//    {
//        // ĐÃ ĐỔI PHÍM NHẢY THÀNH MŨI TÊN LÊN HOẶC PHÍM W
//        if (Level4Director.isPlaying && isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
//        {
//            Jump();
//        }
//    }

//    void Jump()
//    {
//        isGrounded = false;

//        // Reset vận tốc Y trước khi nhảy để tránh lỗi nhảy dồn
//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

//        // Đẩy con ngựa lên trời!
//        rb.AddForce(Vector2.up * lucNhay, ForceMode2D.Impulse);

//        if (tiengNguaHi != null) audioSource.PlayOneShot(tiengNguaHi);
//    }

//    // ==========================================
//    // KIỂM TRA CHẠM ĐẤT ĐỂ CHO NHẢY TIẾP
//    // ==========================================
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        // Khi rơi xuống đụng trúng cái sàn vô hình (Nhớ gắn tag "Ground" cho cái InvisibleGround)
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            if (!isGrounded) // Nếu trước đó đang ở trên không
//            {
//                isGrounded = true;
//                if (tiengDatChan != null) audioSource.PlayOneShot(tiengDatChan);
//            }
//        }
//    }
//}

using UnityEngine;
using UnityEngine.EventSystems; 

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NguaJump : MonoBehaviour
{
    public float lucNhay = 15f;
    public AudioClip tiengNguaHi;
    public AudioClip tiengDatChan;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Kiểm tra điều kiện: Đang chơi + Đang trên mặt đất
        if (Level4Director.isPlaying && isGrounded)
        {
            // 1. Nhận phím cứng trên PC (Để bạn dễ test)
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            // 2. Nhận thao tác CHẠM trên màn hình Mobile
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Nếu vừa mới chạm ngón tay xuống màn hình
                if (touch.phase == TouchPhase.Began)
                {
                    // KIỂM TRA QUAN TRỌNG: Ngón tay có đang bấm vào UI (Hộp thoại) không?
                    // Nếu đang bấm vào Hộp thoại để next chữ -> KHÔNG CHO NHẢY
                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        // Kiểm tra thêm: Có đang ở trong hội thoại không?
                        if (DialogueManager.instance == null || !DialogueManager.instance.isTalking)
                        {
                            Jump();
                        }
                    }
                }
            }

            // 3. (Tùy chọn) Chạm chuột trái khi test trên PC cũng nhảy
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (DialogueManager.instance == null || !DialogueManager.instance.isTalking)
                    {
                        Jump();
                    }
                }
            }
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
        // Nhớ đảm bảo cái sàn của bạn có gắn tag là "Ground" nhé
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded)
            {
                isGrounded = true;
                if (tiengDatChan != null) audioSource.PlayOneShot(tiengDatChan);
            }
        }
    }
}