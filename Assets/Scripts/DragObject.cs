//using UnityEngine;

//[RequireComponent(typeof(Collider2D))]
//public class DragObject : MonoBehaviour
//{
//    public AudioClip dragSound;
//    private Vector3 offset;
//    private bool isPlayingSound = false;

//    // Khi người chơi BẤM CHUỘT XUỐNG cục đá
//    void OnMouseDown()
//    {
//        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

//        if (dragSound != null && !isPlayingSound)
//        {
//            AudioSource.PlayClipAtPoint(dragSound, Camera.main.transform.position);
//            isPlayingSound = true;
//        }
//    }

//    // Khi người chơi GIỮ VÀ DI CHUYỂN CHUỘT
//    void OnMouseDrag()
//    {
//        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
//        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
//    }

//    // Khi người chơi NHẢ CHUỘT RA
//    void OnMouseUp()
//    {
//        isPlayingSound = false;
//    }
//}

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragObject : MonoBehaviour
{
    public AudioClip dragSound; // Kéo tiếng cọ xát đá vào đây
    private Vector3 offset;
    private AudioSource audioSource;

    void Start()
    {
        // Tự động tạo cái Loa cho cục đá
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = dragSound;
        audioSource.loop = true; // Lặp lại tiếng rột rột
        audioSource.playOnAwake = false; // Không tự kêu lúc mới vào
    }

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

        // NẾU CỤC ĐÁ ĐANG BỊ KÉO DI CHUYỂN -> PHÁT ÂM THANH
        if (Vector3.Distance(transform.position, newPosition) > 0.01f)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else // Chuột đứng im thì tắt tiếng
        {
            if (audioSource.isPlaying) audioSource.Pause();
        }

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    void OnMouseUp()
    {
        if (audioSource.isPlaying) audioSource.Stop(); // Nhả chuột ra là im lặng
    }
}