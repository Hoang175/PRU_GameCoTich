//using UnityEngine;

//[RequireComponent(typeof(Collider2D))]
//public class CaBong : MonoBehaviour
//{
//    [SerializeField] string playerTag = "Player";

//    void Start()
//    {
//        GetComponent<Collider2D>().isTrigger = true;
//    }

//    //private void OnTriggerEnter2D(Collider2D collision)
//    //{
//    //    if (collision.CompareTag(playerTag))
//    //    {
//    //        FishCollector collector = collision.GetComponent<FishCollector>();

//    //        if (collector != null)
//    //        {
//    //            collector.AddFish(); 
//    //            Destroy(gameObject); 
//    //        }
//    //    }
//    //}
//}

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CaBong : MonoBehaviour
{
    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
}