using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_Scripts
{
    public static class SceneNavigator
    {

        public static void GoToMenu()
        {
            SceneManager.LoadScene("Scenes/MainMenu");
        }

        public static void StartGame()
        {
            SceneManager.LoadScene("Scenes/MainScene");

        }

        public static void ExitApp()
        {
            Application.Quit();
        }
    }
}