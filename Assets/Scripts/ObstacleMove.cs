//using UnityEngine;

//public class ObstacleMove : MonoBehaviour
//{
//    public float speed = 5f; // Sẽ được Spawner truyền vào cho bằng tốc độ Map

//    void Update()
//    {
//        // Chỉ trôi khi game đang chạy
//        if (Level4Director.isPlaying)
//        {
//            transform.Translate(Vector3.left * speed * Time.deltaTime);

//            // Trôi ra khỏi màn hình bên trái (Tọa độ X < -20) thì tự hủy để không nặng máy
//            if (transform.position.x < -20f)
//            {
//                Destroy(gameObject);
//            }
//        }
//    }
//}

using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public float speed = 5f;
    private bool daTinhDiem = false; // Đánh dấu xem cục này đã cộng điểm chưa

    void Update()
    {
        if (Level4Director.isPlaying)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            // ============================================
            // NẾU TRÔI RA SAU LƯNG TẤM (Ví dụ Tấm đang đứng ở tọa độ X = -6)
            // ============================================
            //if (!daTinhDiem && transform.position.x < -7f)
            //{
            //    daTinhDiem = true; // Khóa lại, không cộng lần 2

            //    // Tìm Đạo diễn máu để bắt nó cộng điểm
            //    PlayerHealth ph = FindFirstObjectByType<PlayerHealth>();
            //    if (ph != null) ph.CongDiem();
            //}
            // Tính điểm NGAY KHI chạm mũi ngựa (Tùy chỉnh số -4.5f cho vừa ý)
            if (!daTinhDiem && transform.position.x < -4.5f)
            {
                daTinhDiem = true;
                PlayerHealth ph = FindFirstObjectByType<PlayerHealth>();
                if (ph != null) ph.CongDiem();
            }

            // Trôi ra khỏi màn hình bên trái thì tự hủy
            if (transform.position.x < -20f)
            {
                Destroy(gameObject);
            }
        }
    }
}