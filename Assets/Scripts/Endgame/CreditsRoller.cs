using UnityEngine;

namespace Endgame
{
    public class CreditsRoller : MonoBehaviour
    {
        [SerializeField] private GameObject knight;
        [SerializeField] private GameObject bird;
        [SerializeField] private GameObject birdAlone;

        void Start()
        {
            if (GameManager.Instance.Victory)
            {
                Destroy(birdAlone);
            }
            else
            {
                Destroy(knight);
                Destroy(bird);
            }
        }
    }
}