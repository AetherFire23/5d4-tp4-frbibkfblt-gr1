using SaveSystem;
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

        void Update()
        {
            if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;

            GameState state = GameManager.Instance.State;

            if (state == GameState.Playing) Pause();
            else if (state != GameState.Paused) return;
            else if (settingsPanel.activeSelf) SwitchUIPanels(settingsPanel, pausePanel);
            else if (quitPanel.activeSelf) SwitchUIPanels(quitPanel, pausePanel);
            else Resume();
        }

        void OnEnable()
        {
            GameManager.Instance.OnStateChanged += OnStateChanged;
        }

        void OnDisable()
        {
            GameManager.Instance.OnStateChanged -= OnStateChanged;
        }

        void OnStateChanged(GameState state)
        {
            player.enabled = state == GameState.Playing;
            pauseRoot.SetActive(state == GameState.Paused);

            if (state != GameState.Paused) return;

            pausePanel.SetActive(true);
            settingsPanel.SetActive(false);
            quitPanel.SetActive(false);
        }

        public void Resume()
        {
            GameManager.Instance.Resume();
        }

        public void Pause()
        {
            GameManager.Instance.Pause();
        }

        public void SwitchUIPanels(GameObject sourcePanel, GameObject targetPanel)
        {
            StartCoroutine(FadeHelper.SwitchPanels(sourcePanel, targetPanel));
        }

        public static void SaveGame()
        {
            Persistance.ClearFoodItems();
            foreach (var foodManager in FindObjectsByType<FoodManager>(FindObjectsSortMode.None))
            {
                Persistance.RegisterFoodItem(foodManager.GetObjectData());
                Persistance.RegisterPlayerData(FindFirstObjectByType<NoclipPlayerController>().GetObjectData());
            }

            Persistance.Save(SceneLoadedListener.saveName);
            ;
        }

        public void ReturnToMenu()
        {
            GameManager.Instance.GoToMenu();
        }

        public void QuitGame()
        {
            GameManager.Instance.ExitApp();
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