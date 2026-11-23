using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    void Start()
    {
        GoToMenu();
    }

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