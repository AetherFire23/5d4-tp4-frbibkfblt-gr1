using System.Collections;
using UnityEngine;

namespace UI_Scripts
{
    public static class FadeHelper
    {
        const float fadeDuration = 0.25f;

        public static IEnumerator SwitchPanels(GameObject from, GameObject to)
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

        public static IEnumerator Fade(CanvasGroup group, float target, float duration = fadeDuration)
        {
            float start = group.alpha;
            float time = 0f;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                group.alpha = Mathf.Lerp(start, target, time / duration);
                yield return null;
            }

            group.alpha = target;
        }
    }
}