using System.Collections;
using AudioScripts;
using Knight.Pure;
using SaveSystem;
using TMPro;
using UI_Scripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Endgame
{
    public class EndingManager : MonoBehaviour
    {
        public float sceneViewTime = 4f;
        public float endFadeTime = 1f;

        public AudioResource victoryMusic;
        public AudioResource defeatMusic;

        public GameObject creditsPanel;
        public TMP_Text creditsText;
        public float creditsSpeed = 80f;

        public CanvasGroup defeatPanel;
        public GameObject[] afflictionIcons;

        public CanvasGroup buttonsPanel;
        public Button loadButton;
        public GameObject replayButton;

        IEnumerator Start()
        {
            if (MusicController.Instance != null)
                MusicController.Instance.Play(GameManager.Instance.Victory ? victoryMusic : defeatMusic);

            yield return new WaitForSeconds(sceneViewTime);
            yield return FadeHelper.Fade(GameManager.Instance.BlackScreen, 1f, endFadeTime);
            yield return GameManager.Instance.Victory ? RollCredits() : ShowDefeat();
            yield return ShowButtons();
        }

        IEnumerator RollCredits()
        {
            creditsPanel.SetActive(true);
            RectTransform credits = creditsText.rectTransform;
            float endY = creditsText.preferredHeight + ((RectTransform)creditsPanel.transform).rect.height;

            while (credits.anchoredPosition.y < endY && !SkipPressed())
            {
                credits.anchoredPosition += Vector2.up * creditsSpeed * Time.deltaTime;
                yield return null;
            }

            creditsPanel.SetActive(false);
        }

        IEnumerator ShowDefeat()
        {
            for (int i = 0; i < afflictionIcons.Length; i++)
                afflictionIcons[i]
                    .SetActive(GameManager.Instance.RemainingAfflictions.Contains((KnightAfflictionTypes)i));

            defeatPanel.alpha = 0f;
            defeatPanel.gameObject.SetActive(true);
            return FadeHelper.Fade(defeatPanel, 1f, endFadeTime);
        }

        IEnumerator ShowButtons()
        {
            bool victory = GameManager.Instance.Victory;
            loadButton.gameObject.SetActive(!victory);
            loadButton.interactable = Persistance.SaveExists(SceneLoadedListener.saveName);
            replayButton.SetActive(victory);

            buttonsPanel.alpha = 0f;
            buttonsPanel.gameObject.SetActive(true);
            return FadeHelper.Fade(buttonsPanel, 1f);
        }

        static bool SkipPressed()
        {
            return Keyboard.current.escapeKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame;
        }

        public void LoadLastSave()
        {
            SceneLoadedListener.loadFromSave = true;
            GameManager.Instance.StartGame();
        }

        public void Replay()
        {
            SceneLoadedListener.loadFromSave = false;
            GameManager.Instance.StartGame();
        }
    }
}