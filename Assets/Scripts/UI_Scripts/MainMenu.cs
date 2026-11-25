using System.Collections;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject continueButton;
        public GameObject settingsPanel;
        public GameObject mainPanel;
        public Slider volumeSlider;
        public float fadeDuration = 0.25f;

        void Start()
        {
            continueButton.SetActive(Persistance.SaveExists("save1"));
            mainPanel.SetActive(true);
            settingsPanel.SetActive(false);

            volumeSlider.value = MusicController.Instance.Volume;
            volumeSlider.onValueChanged.AddListener(MusicController.Instance.SetVolume);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Continue()
        {
            SceneNavigator.StartGame();
            SceneLoadedListener.loadFromSave = true;
            SceneLoadedListener.saveName = "save1";
        }

        public void NewGame()
        {
            SceneNavigator.StartGame();
            SceneLoadedListener.loadFromSave = false;
        }

        public void OpenSettings()
        {
            StartCoroutine(MenuFadeHelper.SwitchPanels(mainPanel, settingsPanel));
        }

        public void CloseSettings()
        {
            StartCoroutine(MenuFadeHelper.SwitchPanels(settingsPanel, mainPanel));
        }


        public void Quit()
        {
            SceneNavigator.ExitApp();
        }
    }
}