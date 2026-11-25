using System.Collections.Generic;
using SaveSystem;
using UI_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadedListener : MonoBehaviour
{
    public const string DEFAULT_NEW_SAVE_GAME_NAME = "save1";
    public static bool loadFromSave = false;
    public static string saveName = DEFAULT_NEW_SAVE_GAME_NAME;

    public bool IsKnightSceneLoaded { get; set; } = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene: " + scene.name);
        LoadData();
    }

    public void LoadData()
    {
        if (!this.IsKnightSceneLoaded)
        {
            return;
        }

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
            PauseMenu.SaveGame();
        }
    }
}
