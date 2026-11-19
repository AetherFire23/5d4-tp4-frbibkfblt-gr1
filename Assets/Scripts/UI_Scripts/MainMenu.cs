using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject continueButton;
        public GameObject settingsPanel;
        public GameObject mainPanel;
        public Slider volumeSlider;
        public float fadeDuration = 0.25f;

        void Start()
        {
            continueButton.SetActive(SaveSystem.SaveExists());
            mainPanel.SetActive(true);
            settingsPanel.SetActive(false);

            volumeSlider.value = MusicController.Instance.Volume;
            volumeSlider.onValueChanged.AddListener(MusicController.Instance.SetVolume);
        }

        public void Continue()
        {
            //somethingThatManagesGameState.loadSave = true;
            SceneNavigator.StartGame();
        }

        public void NewGame()
        {
            SceneNavigator.StartGame();
        }

        public void OpenSettings()
        {
            StartCoroutine(SwitchPanels(mainPanel, settingsPanel));
        }

        public void CloseSettings()
        {
            StartCoroutine(SwitchPanels(settingsPanel, mainPanel));
        }

        IEnumerator SwitchPanels(GameObject from, GameObject to)
        {
            CanvasGroup fromGroup = from.GetComponent<CanvasGroup>();
            CanvasGroup toGroup = to.GetComponent<CanvasGroup>();

            fromGroup.interactable = false;
            toGroup.interactable = false;

            yield return Fade(fromGroup, 0f);
            from.SetActive(false);

            toGroup.alpha = 0f;
            to.SetActive(true);
            yield return Fade(toGroup, 1f);

            toGroup.interactable = true;
        }

        IEnumerator Fade(CanvasGroup group, float target)
        {
            float start = group.alpha;
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.unscaledDeltaTime;
                group.alpha = Mathf.Lerp(start, target, time / fadeDuration);
                yield return null;
            }

            group.alpha = target;
        }

        public void Quit()
        {
            SceneNavigator.ExitApp();
        }
    }
}
