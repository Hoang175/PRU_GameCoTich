//using UnityEngine;
//using UnityEngine.UI; // Để gọi Image UI

//[RequireComponent(typeof(Collider2D))]
//public class ClickToFind : MonoBehaviour
//{
//    [Header("UI Bóng đen trên Taskbar")]
//    public Image uiSilhouette; // Kéo cái Image màu đen tương ứng trên Canvas vào đây!

//    [Header("Âm thanh & Hiệu ứng")]
//    public AudioClip findSound; // Tiếng Ting!

//    // Biến đếm tổng số đồ đã tìm thấy
//    public static int itemsFound = 0;

//    void Start()
//    {
//        itemsFound = 0; // Reset điểm mỗi khi vào màn
//    }

//    // Khi người chơi CLICK CHUỘT TRÚNG món đồ trên Map
//    void OnMouseDown()
//    {
//        // 1. Phát tiếng kêu
//        if (findSound != null) AudioSource.PlayClipAtPoint(findSound, Camera.main.transform.position);

//        // 2. Thắp sáng cái bóng đen trên UI (Đổi từ màu đen thành màu trắng = Hiện màu gốc)
//        if (uiSilhouette != null)
//        {
//            uiSilhouette.color = Color.white;
//        }

//        // 3. Cộng điểm
//        itemsFound++;

//        // 4. Kiểm tra xem đã tìm đủ 4 món chưa
//        //if (itemsFound >= 4)
//        //{
//        //    Debug.Log(">>> ĐÃ TÌM ĐỦ 4 MÓN ĐỒ! <<<");
//        //    WinLevel();
//        //}
//        if (itemsFound >= 4)
//        {
//            Level3Controller controller = FindFirstObjectByType<Level3Controller>();
//            if (controller != null) controller.TransformTam();
//        }

//        // 5. Làm món đồ trên Map biến mất
//        gameObject.SetActive(false);
//    }

//} 

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class ClickToFind : MonoBehaviour
{
    [Header("UI Bóng đen trên Taskbar")]
    public Image uiSilhouette;

    [Header("Âm thanh & Hiệu ứng")]
    public AudioClip findSound;

    // === THÊM MỚI: HỆ THỐNG VẬT CẢN ===
    [Header("Chướng ngại vật che giấu (Nếu có)")]
    public Transform vatCan; // Kéo Cục đá, Đống rơm che món đồ này vào đây
    public float khoangCachAnToan = 1.5f; // Khoảng cách tối thiểu phải kéo vật cản ra xa

    public static int itemsFound = 0;

    void Start()
    {
        itemsFound = 0;
    }

    void OnMouseDown()
    {
        // === KIỂM TRA XEM CÓ BỊ CHE KHÔNG ===
        if (vatCan != null)
        {
            // Nếu khoảng cách giữa Món đồ và Vật cản đang quá gần -> Không cho nhặt!
            float distance = Vector2.Distance(transform.position, vatCan.position);
            if (distance < khoangCachAnToan)
            {
                Debug.Log("Món đồ đang bị che! Phải kéo " + vatCan.name + " ra xa!");
                return; // Ngắt lệnh, không làm gì cả
            }
        }

        // Bắt đầu nhặt đồ
        if (findSound != null) AudioSource.PlayClipAtPoint(findSound, Camera.main.transform.position);

        if (uiSilhouette != null) uiSilhouette.color = Color.white;

        itemsFound++;

        if (itemsFound >= 4)
        {
            Level3Controller controller = FindFirstObjectByType<Level3Controller>();
            if (controller != null) controller.TransformTam();
        }

        gameObject.SetActive(false);
    }
}