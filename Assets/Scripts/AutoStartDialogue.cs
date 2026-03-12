//using UnityEngine;

//public class AutoStartDialogue : MonoBehaviour
//{
//    [Header("Nội dung Tự nhủ")]
//    [TextArea(2, 5)]
//    public string[] lines;

//    void Start()
//    {
//        // Gọi thẳng DialogueManager để chạy thoại ngay khi load màn
//        if (DialogueManager.instance != null)
//        {
//            DialogueManager.instance.StartDialogue(lines);
//        }
//    }
//}

using System.Collections; // BẮT BUỘC PHẢI CÓ DÒNG NÀY ĐỂ DÙNG BỘ ĐẾM GIỜ
using UnityEngine;

public class AutoStartDialogue : MonoBehaviour
{
    [Header("Nội dung Tự nhủ")]
    [TextArea(2, 5)]
    public string[] lines;

    // Đổi Start thành IEnumerator Start
    IEnumerator Start()
    {
        // Đợi 0.5 giây cho khung thoại, nhân vật load lên hoàn chỉnh
        yield return new WaitForSeconds(0.5f);

        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(lines);
        }
    }
}