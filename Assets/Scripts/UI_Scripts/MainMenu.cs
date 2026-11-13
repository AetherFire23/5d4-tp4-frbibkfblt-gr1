using UnityEngine;

namespace UI_Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject continueButton;
        public GameObject settingsPanel;
        public GameObject mainPanel;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            continueButton.SetActive(SaveSystem.SaveExists());
            settingsPanel.SetActive(false);
        }
    
        public void Continue()
        {
            //somethingThatManagesGameState.loadSave = true;
            SceneNavigator.StartGame();
        }
    
        public void OpenSettings()
        {
            mainPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }
    }
}
