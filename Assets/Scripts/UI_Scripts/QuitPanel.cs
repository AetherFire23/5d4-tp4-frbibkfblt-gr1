using UnityEngine;

namespace UI_Scripts
{
    public class QuitPanel : MonoBehaviour
    {
        public GameObject panel;
        public GameObject returnPanel;

        public void Open()
        {
            StartCoroutine(FadeHelper.SwitchPanels(returnPanel, panel));
        }

        public void Close()
        {
            StartCoroutine(FadeHelper.SwitchPanels(panel, returnPanel));
        }

        public void GoToMenu()
        {
            GameManager.Instance.GoToMenu();
        }

        public void Quit()
        {
            GameManager.Instance.ExitApp();
        }
    }
}