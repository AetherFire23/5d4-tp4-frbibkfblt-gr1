using System;
using SaveSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI_Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseRoot;
        public GameObject pausePanel;
        public GameObject settingsPanel;
        public GameObject quitPanel;
        public NoclipPlayerController player;

        void Start()
        {
            pauseRoot.SetActive(false);
        }

        void Update()
        {
            if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;

            if (!pauseRoot.activeSelf) Pause();
            else if (settingsPanel.activeSelf) SwitchUIPanels(settingsPanel, pausePanel);
            else if (quitPanel.activeSelf) SwitchUIPanels(quitPanel, pausePanel);
            else Resume();
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            player.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            pauseRoot.SetActive(false);
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            player.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pausePanel.SetActive(true);
            settingsPanel.SetActive(false);
            quitPanel.SetActive(false);
            pauseRoot.SetActive(true);
        }

        public void SwitchUIPanels(GameObject sourcePanel, GameObject targetPanel)
        {
            StartCoroutine(MenuFadeHelper.SwitchPanels(sourcePanel, targetPanel));
        }

        public static void SaveGame()
        {
            Persistance.ClearFoodItems();
            foreach (var foodManager in FindObjectsByType<FoodManager>(FindObjectsSortMode.None))
            {
                Persistance.RegisterFoodItem(foodManager.GetObjectData());
                Persistance.RegisterPlayerData(FindFirstObjectByType<NoclipPlayerController>().GetObjectData());
            }
            Persistance.Save(SceneLoadedListener.saveName);;
        }

        public void ReturnToMenu()
        {
            Time.timeScale = 1f;
            SceneNavigator.GoToMenu();
        }

        public void QuitGame()
        {
            SceneNavigator.ExitApp();
        }

        public void OpenQuitPanel()
        {
            SwitchUIPanels(pausePanel, quitPanel);
        }

        public void CloseQuitPanel()
        {
            SwitchUIPanels(quitPanel, pausePanel);
        }

        public void OpenSettings()
        {
            SwitchUIPanels(pausePanel, settingsPanel);
        }

        public void CloseSettings()
        {
            SwitchUIPanels(settingsPanel, pausePanel);
        }
    }
}