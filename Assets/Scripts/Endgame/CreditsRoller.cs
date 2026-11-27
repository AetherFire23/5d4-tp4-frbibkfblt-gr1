using UnityEngine;

namespace Endgame
{
    public class CreditsRoller : MonoBehaviour
    {
        [SerializeField] private Animator birdAnimator;
        [SerializeField] private GameObject knight;

        void Start()
        {
            var endgameScript = FindFirstObjectByType<EndGameDontDestroyOnLoad>();
            if (!endgameScript.IsWin)
            {
                Destroy(knight);
                birdAnimator.SetBool("flying", true);
            }
        }
    }
}