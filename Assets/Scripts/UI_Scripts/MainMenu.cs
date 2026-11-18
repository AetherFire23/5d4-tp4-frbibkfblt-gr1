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

        void Start()
        {
            continueButton.SetActive(SaveSystem.SaveExists());
            mainPanel.SetActive(true);
            settingsPanel.SetActive(false);

            volumeSlider.value = MusicController.Instance.Volume;
            volumeSlider.onValueChanged.AddListener(MusicController.Instance.SetVolume);
        }

        public void Continue()
        {
            //somethingThatManagesGameState.loadSave = true;
            SceneNavigator.StartGame();
        }

        public void NewGame()
        {
            SceneNavigator.StartGame();
        }

        public void OpenSettings()
        {
            mainPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            settingsPanel.SetActive(false);
            mainPanel.SetActive(true);
        }

        public void Quit()
        {
            SceneNavigator.ExitApp();
        }
    }
}
