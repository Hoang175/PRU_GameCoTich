using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HE181245_DaoHuyHoang_UntoldTalesFairyTales
{
    public class IngameMenuManager : MonoBehaviour
    {
        [Header("Giao diện UI")]
        public GameObject menuPanel;
        public GameObject settingPanel;

        [Header("Cài đặt (Settings)")]
        public Slider volumeSlider;
        public Toggle mobileUIToggle;

        private GameObject canvasMobile; 

        void Start()
        {
            if (menuPanel != null) menuPanel.SetActive(false);
            if (settingPanel != null) settingPanel.SetActive(false);

            canvasMobile = GameObject.Find("CanvasMobile");

            if (volumeSlider != null)
            {
                volumeSlider.value = AudioListener.volume;
                volumeSlider.onValueChanged.AddListener(SetVolume);
            }

            if (mobileUIToggle != null && canvasMobile != null)
            {
                mobileUIToggle.isOn = canvasMobile.activeSelf;
                mobileUIToggle.onValueChanged.AddListener(ToggleMobileUI);
            }
        }

        public void ToggleMenu()
        {
            if (menuPanel != null)
            {
                bool isOpening = !menuPanel.activeSelf;
                menuPanel.SetActive(isOpening);

                Time.timeScale = isOpening ? 0f : 1f;

                if (!isOpening && settingPanel != null) settingPanel.SetActive(false);
            }
        }

        // Bật / Tắt bảng Setting
        public void ToggleSetting()
        {
            if (settingPanel != null)
            {
                settingPanel.SetActive(!settingPanel.activeSelf);
            }
        }

        public void SetVolume(float value)
        {
            AudioListener.volume = value;
        }

        public void ToggleMobileUI(bool isOn)
        {
            if (canvasMobile != null)
            {
                canvasMobile.SetActive(isOn);
            }
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}