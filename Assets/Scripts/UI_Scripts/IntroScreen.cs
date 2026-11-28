using System.Collections;
using UnityEngine;

namespace UI_Scripts
{
    public class IntroScreen : MonoBehaviour
    {
        public float displayTime = 3f;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(displayTime);
            GameManager.Instance.GoToMenu();
        }
    }
}