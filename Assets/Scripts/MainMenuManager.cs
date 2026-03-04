using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [Header("Giao diện Cài đặt")]
    public GameObject settingsPanel; 
    public Slider volumeSlider;      

    void Start()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
        }
    }

    public void PlayGame()
    {
        Debug.Log("Đang tải màn chơi chính...");
        SceneManager.LoadScene("fullMap1");
    }


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
        Debug.Log("Đang thoát game...");
        Application.Quit();
    }
}