using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerHealth : MonoBehaviour
{
    [Header("Cài đặt Máu & Điểm")]
    public int maxHealth = 3;
    private int currentHealth;
    public Image[] timUI;
    public int diemCanDat = 15;
    public static int diemHienTai = 0;
    public TextMeshProUGUI textDiem;

    [Header("Giao diện Thua/Thắng")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI titleText; // Chữ "GAME OVER" to đùng
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverMessageText;
    public TextMeshProUGUI buttonText; // Chữ "REPLAY" trên nút bấm

    [Header("Chuyển Màn")]
    public VideoPlayer videoPlayer; // Kéo Video Player vào đây
    public string nextScene = "Lv5_1_ThuHai"; // Tên map màn 5
    private bool isWon = false; // Biến kiểm tra xem đang thắng hay thua

    void Start()
    {
        currentHealth = maxHealth;
        diemHienTai = 0;
        isWon = false;
        CapNhatUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            TruMau();
            Destroy(collision.gameObject);
        }
    }

    void TruMau()
    {
        currentHealth--;
        CapNhatUI();
        if (currentHealth <= 0) ThuaGame();
    }

    public void CongDiem()
    {
        diemHienTai++;
        CapNhatUI();
        if (diemHienTai >= diemCanDat) ThangGame();
    }

    void CapNhatUI()
    {
        for (int i = 0; i < timUI.Length; i++)
        {
            timUI[i].enabled = (i < currentHealth);
        }
        if (textDiem != null) textDiem.text = "Score: " + diemHienTai + "/" + diemCanDat;
    }

    void ThuaGame()
    {
        Level4Director.isPlaying = false;
        Time.timeScale = 0f;
        isWon = false;

        if (titleText != null) titleText.text = "GAME OVER";
        if (buttonText != null) buttonText.text = "REPLAY";
        if (gameOverScoreText != null) gameOverScoreText.text = "Your Score: " + diemHienTai;
        if (gameOverMessageText != null) gameOverMessageText.text = "You need " + diemCanDat + " points to pass.\nTry harder!";

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    void ThangGame()
    {
        Level4Director.isPlaying = false;
        Time.timeScale = 0f;
        isWon = true; // Đánh dấu là ĐÃ THẮNG

        if (titleText != null) titleText.text = "VICTORY!";
        if (buttonText != null) buttonText.text = "NEXT LEVEL";
        if (gameOverScoreText != null) gameOverScoreText.text = "Your Score: " + diemHienTai;
        if (gameOverMessageText != null) gameOverMessageText.text = "Excellent!\nYou have reached the festival!";

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // Nút bấm bây giờ sẽ đa năng: Thua thì chơi lại, Thắng thì xem video qua màn!
    public void OnButtonClick()
    {
        Time.timeScale = 1f; // Phải mở khóa thời gian thì video mới chạy được
        gameOverPanel.SetActive(false); // Ẩn bảng đi

        if (isWon)
        {
            // Bấm lúc thắng -> Qua màn
            StartCoroutine(PlayVideoAndNextLevel());
        }
        else
        {
            // Bấm lúc thua -> Load lại Scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //IEnumerator PlayVideoAndNextLevel()
    //{
    //    // Tắt con ngựa và nền đi cho đỡ vướng Video
    //    GetComponent<SpriteRenderer>().enabled = false;
    //    GameObject.Find("MapManager").SetActive(false);

    //    if (videoPlayer != null)
    //    {
    //        videoPlayer.Play();
    //        yield return new WaitForSeconds((float)videoPlayer.length + 0.5f);
    //    }
    //    else
    //    {
    //        yield return new WaitForSeconds(1f); // Phòng hờ nếu chưa có video
    //    }

    //    Debug.Log(">>> CHUYỂN SANG MÀN 5: THỬ HÀI <<<");
    //    SceneManager.LoadScene(nextScene);
    //}

    IEnumerator PlayVideoAndNextLevel()
    {
        // Tắt con ngựa
        GetComponent<SpriteRenderer>().enabled = false;

        // Tắt nền Map (Tên trong Hierarchy của bạn là Background)
        GameObject bg = GameObject.Find("Background");
        if (bg != null) bg.SetActive(false);

        // Tắt luôn mấy chướng ngại vật đang trôi dở trên màn hình
        GameObject[] quaiVat = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject quai in quaiVat) { Destroy(quai); }

        // Phát Video
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            // Đợi video chạy xong
            yield return new WaitForSeconds((float)videoPlayer.length + 0.5f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        Debug.Log(">>> CHUYỂN SANG MÀN 5: THỬ HÀI <<<");
        SceneManager.LoadScene(nextScene);
    }
}