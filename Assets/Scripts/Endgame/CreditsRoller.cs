using UnityEngine;


public class CreditsRoller : MonoBehaviour
{
    [SerializeField] private GameObject knight;
    void Start()
    {
        var endgameScript = FindFirstObjectByType<EndGameDontDestroyOnLoad>();
        if (!endgameScript.IsWin)
        {
            Destroy(knight);
        }
    }

    void Update()
    {
    }
}