using Knight.Pure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Endgame
{
    public class EndGameDontDestroyOnLoad : MonoBehaviour
    {
        public bool IsWin { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void DoWinningEndgame(KnightState knightState)
        {
            Debug.Log("SHOULD ENTER END GAME");
            this.IsWin = true;
            SceneManager.LoadScene("EndgameScene", LoadSceneMode.Single);
        }

        public void DoLosingEndgame(KnightState knightState)
        {
            this.IsWin = false;
            SceneManager.LoadScene("EndgameScene", LoadSceneMode.Single);
        }
    }
}