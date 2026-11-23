using UnityEngine;

namespace UI_Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject continueButton;
        public GameObject settingsPanel;
        public GameObject mainPanel;

        void Start()
        {
            continueButton.SetActive(SaveSystem.SaveExists());
            mainPanel.SetActive(true);
            settingsPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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