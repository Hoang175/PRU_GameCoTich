//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class MainMenuManager : MonoBehaviour
//{

//    [Header("Giao diện Cài đặt")]
//    public GameObject settingsPanel; 
//    public Slider volumeSlider;      

//    void Start()
//    {
//        if (settingsPanel != null)
//        {
//            settingsPanel.SetActive(false);
//        }

//        if (volumeSlider != null)
//        {
//            volumeSlider.value = AudioListener.volume;
//        }
//    }

//    public void PlayGame()
//    {
//        Debug.Log("Đang tải màn chơi chính...");
//        SceneManager.LoadScene("fullMap1");
//    }


//    public void OpenSettings()
//    {
//        if (settingsPanel != null) settingsPanel.SetActive(true);
//    }
//    public void CloseSettings()
//    {
//        if (settingsPanel != null) settingsPanel.SetActive(false); 
//    }

//    public void SetVolume(float volume)
//    {
//        AudioListener.volume = volume; 
//    }


//    public void QuitGame()
//    {
//        Debug.Log("Đang thoát game...");
//        Application.Quit();
//    }
//}


using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject settingsPanel;
    public GameObject levelSelectPanel; // Kéo cục Empty "LevelSelectPanel" vào đây!

    [Header("Settings Controls")]
    public Slider volumeSlider;

    void Start()
    {
        // Hide panels on start
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
        }
    }

    // ==========================================
    // MAIN FUNCTIONS
    // ==========================================

    // Called when the main "PLAY" button is pressed to show level select
    public void OpenLevelSelect()
    {
        Debug.Log("Opening Level Selection...");
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
    }

    // ==========================================
    // LEVEL LOADING FUNCTIONS
    // ==========================================
    // Gán 6 hàm này vào 6 cái Banner/Nút bấm chọn màn cụ thể nhé!

    public void LoadLevel1()
    {
        Debug.Log("Loading Level 1: Fishing Task...");
        SceneManager.LoadScene("fullMap1"); // Retain your exact scene name
    }

    public void LoadLevel2()
    {
        Debug.Log("Loading Level 2: Stealthy Entry...");
        SceneManager.LoadScene("Lv2_1_DuongVao"); // Retain your exact scene name
    }

    public void LoadLevel3()
    {
        Debug.Log("Loading Level 3: Bird Magic...");
        SceneManager.LoadScene("Lv3_1_Phepmau"); // Retain your exact scene name
    }

    public void LoadLevel4()
    {
        Debug.Log("Loading Level 4: Festival Gallop...");
        SceneManager.LoadScene("Lv4_1_TrayHoi"); // Retain your exact scene name
    }

    public void LoadLevel5()
    {
        Debug.Log("Loading Level 5: The Golden Slipper...");
        // Ensure you add these future scenes to Build Settings!
        SceneManager.LoadScene("Lv5_1_ThuHai");
    }

    public void LoadLevel6()
    {
        Debug.Log("Loading Level 6: Royal Union...");
        SceneManager.LoadScene("Lv6_1_Finale");
    }

    // ==========================================
    // SETTINGS & QUIT
    // ==========================================

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void QuitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}