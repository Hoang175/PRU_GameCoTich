//using UnityEngine;

//public class MapScroller : MonoBehaviour
//{
//    //public float speed = 5f;
//    //public int soLuongMap = 3;

//    //private float chieuRongMap;
//    //private Vector3 viTriBanDau;

//    //void Start()
//    //{
//    //    // Tự động đo chiều rộng siêu chuẩn xác
//    //    chieuRongMap = GetComponent<SpriteRenderer>().bounds.size.x;
//    //    viTriBanDau = transform.position;
//    //}

//    //void Update()
//    //{
//    //    if (Level4Director.isPlaying)
//    //    {
//    //        // Di chuyển tuyệt đối theo trục X của Thế giới (World Space)
//    //        transform.position += Vector3.left * speed * Time.deltaTime;

//    //        // Nếu map trôi qua khỏi vị trí ban đầu đúng bằng 1 lần chiều rộng của nó
//    //        if (transform.position.x <= viTriBanDau.x - chieuRongMap)
//    //        {
//    //            // Dịch chuyển nó lên tít đằng trước nối đuôi (Cộng đúng 3 lần chiều rộng)
//    //            transform.position += new Vector3(chieuRongMap * soLuongMap, 0, 0);
//    //        }
//    //    }
//    //}

//    public Transform mainCam;
//    public Transform leftMap0;
//    public Transform middleMap01;
//    public Transform rightMap02;

//     public float lenght;

//    private void Update()
//    {
//        if(mainCam.position.x > leftMap0.position.x)
//        {
//            UpdateBackgroundPosition(Vector3.right);
//        }
//    }

//    void UpdateBackgroundPosition(Vector3 direction)
//    {
//        rightMap02.position += leftMap0.position + middleMap01.position + direction * lenght ;
//        Transform temp = lenght;

//    }
//}

using UnityEngine;

public class MapScroller : MonoBehaviour
{
    [Header("Tốc độ trôi")]
    public float speed = 5f;

    [Header("Kéo 3 tấm Map vào đây (Từ Trái sang Phải)")]
    public Transform leftMap;   // Kéo Map_0 vào đây
    public Transform middleMap; // Kéo Map_1 vào đây
    public Transform rightMap;  // Kéo Map_2 vào đây

    private float length; // Chiều rộng 1 tấm map

    void Start()
    {
        // Tự động đo chiều rộng của tấm map đầu tiên
        length = leftMap.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Chỉ chạy khi Tấm đã lên Xích Thố
        if (Level4Director.isPlaying)
        {
            // 1. Di chuyển cả 3 tấm Map sang TRÁI
            leftMap.position += Vector3.left * speed * Time.deltaTime;
            middleMap.position += Vector3.left * speed * Time.deltaTime;
            rightMap.position += Vector3.left * speed * Time.deltaTime;

            // 2. Nếu tấm LeftMap trôi khuất hẳn ra khỏi màn hình bên trái
            if (leftMap.position.x <= -length)
            {
                UpdateBackgroundPosition();
            }
        }
    }

    void UpdateBackgroundPosition()
    {
        // Bước A: Bốc tấm LeftMap nhấc lên đặt ra sau lưng tấm RightMap
        leftMap.position = rightMap.position + new Vector3(length, 0, 0);

        // Bước B: THUẬT TOÁN HOÁN VỊ (Hoán đổi tên gọi cho nhau)
        // Giống như việc bạn có 3 ly nước, muốn đổi chỗ nước phải dùng ly thứ 4 (temp)
        Transform temp = leftMap;

        leftMap = middleMap;      // Tấm ở Giữa giờ chính thức thành tấm Ngoài Cùng Trái
        middleMap = rightMap;     // Tấm Phải giờ thành tấm Giữa
        rightMap = temp;          // Tấm Trái (vừa bị nhấc ra sau) giờ chính thức là tấm Phải
    }
}