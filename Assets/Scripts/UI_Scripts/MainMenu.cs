using SaveSystem;
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
            continueButton.SetActive(Persistance.SaveExists("save1"));
            mainPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }

        public void Continue()
        {
            GameManager.Instance.StartGame();
            SceneLoadedListener.loadFromSave = true;
            SceneLoadedListener.saveName = "save1";
        }

        public void NewGame()
        {
            GameManager.Instance.StartGame();
            SceneLoadedListener.loadFromSave = false;
        }

        public void OpenSettings()
        {
            StartCoroutine(FadeHelper.SwitchPanels(mainPanel, settingsPanel));
        }

        public void CloseSettings()
        {
            StartCoroutine(FadeHelper.SwitchPanels(settingsPanel, mainPanel));
        }


        public void Quit()
        {
            GameManager.Instance.ExitApp();
        }
    }
}