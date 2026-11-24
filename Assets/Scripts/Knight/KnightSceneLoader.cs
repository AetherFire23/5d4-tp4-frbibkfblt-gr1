using UnityEngine;
using UnityEngine.SceneManagement;

public class KnightSceneLoader : MonoBehaviour
{
    void Start()
    {
        Scene knightScene = SceneManager.GetSceneByName("KnightScene");

        if (knightScene.IsValid())
        {
            Debug.Log("Scene KnightScene is already added to ne scene. ");
        }
        else
        {
            SceneManager.LoadScene("KnightScene", LoadSceneMode.Additive);
        }
    }

    void Update()
    {
    }
}