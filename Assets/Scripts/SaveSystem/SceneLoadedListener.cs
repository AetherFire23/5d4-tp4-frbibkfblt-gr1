using System.Collections.Generic;
using SaveSystem;
using UI_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadedListener : MonoBehaviour
{
    public static bool loadFromSave = false;
    public static string saveName = null;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Loaded: {scene.name}");
        if (loadFromSave)
        {
            Persistance.SetFoodItemFactory(GetComponent<FoodItemFactory>());
            Persistance.Load(saveName);
        }
        else
        {
            foreach (ItemSpotManager itemSpotManager in FindObjectsByType<ItemSpotManager>(FindObjectsSortMode.None))
            {
                itemSpotManager.Create();
            }
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            FindAnyObjectByType<PauseMenu>().SaveGame();
        }

    }

}
