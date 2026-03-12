using UnityEngine;

public class SpriteGhost : MonoBehaviour
{
    private SpriteRenderer sr;
    public float fadeSpeed = 2f; 
    private Color color;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
    }

    void Update()
    {
        // Giảm dần độ trong suốt (alpha) theo thời gian
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;

        // Nếu mờ hẳn rồi thì tự xóa bản thân để tránh rác RAM
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}