using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Knight.Pure;
using UI_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const float fadeTime = 0.5f;

    public CanvasGroup BlackScreen { get; private set; }
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }

    public bool Victory { get; private set; }
    public List<KnightAfflictionTypes> RemainingAfflictions { get; private set; } = new();

    void Awake()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        BlackScreen = gameObject.AddComponent<CanvasGroup>();
        BlackScreen.alpha = 0f;
        BlackScreen.blocksRaycasts = false;

        Image black = new GameObject("BlackScreen").AddComponent<Image>();
        black.transform.SetParent(transform, false);
        black.color = Color.black;
        black.rectTransform.anchorMin = Vector2.zero;
        black.rectTransform.anchorMax = Vector2.one;
        black.rectTransform.offsetMin = Vector2.zero;
        black.rectTransform.offsetMax = Vector2.zero;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
    }

    public event Action<GameState> OnStateChanged;

    //cet attribut là load le gamemanager et le gere en statique peu importe on load quelle scène dans l'editor
    //personne a besoin de modifier sa scene pour avoir un gameManager, byebye mergeConflict
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
        GameObject managerObject = new GameObject("GameManager");
        DontDestroyOnLoad(managerObject);
        Instance = managerObject.AddComponent<GameManager>();
    }

    //check le state sauf si on est dans une scène additive (la knight scene de fred)
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive) return;

        if (scene.name == "MainScene") SetState(GameState.Playing);
        else if (scene.name == "EndgameScene") SetState(GameState.Ending);
        else SetState(GameState.Menu);
    }

    //is this react? change ce que j'ai à changer, timescale, état de la souris, etc. et trigger les listeners
    void SetState(GameState state)
    {
        State = state;

        Time.timeScale = state == GameState.Paused ? 0f : 1f;

        bool playing = state == GameState.Playing;
        Cursor.lockState = playing ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !playing;

        OnStateChanged?.Invoke(state);
    }

    public void Pause()
    {
        if (State == GameState.Playing) SetState(GameState.Paused);
    }

    public void Resume()
    {
        if (State == GameState.Paused) SetState(GameState.Playing);
    }

    public void GoToMenu()
    {
        StartCoroutine(FadeAndLoad("Scenes/MainMenu"));
    }

    public void StartGame()
    {
        StartCoroutine(FadeAndLoad("Scenes/MainScene"));
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void EndGame(KnightState knight)
    {
        Debug.Log("SHOULD ENTER END GAME");
        Victory = knight.IsFullyCured;
        RemainingAfflictions = knight.Afflictions
            .Where(affliction => !affliction.IsCured)
            .Select(affliction => affliction.KnightAfflictionType)
            .ToList();

        SetState(GameState.Ending);
        StartCoroutine(FadeAndLoad("Scenes/EndgameScene"));
    }

    [ContextMenu("Test Defeat")]
    void TestDefeat()
    {
        EndGame(new KnightState());
    }

    [ContextMenu("Test Victory")]
    void TestVictory()
    {
        KnightState knight = new KnightState();
        knight.Afflictions.ForEach(affliction => affliction.Cure());
        EndGame(knight);
    }

    IEnumerator FadeAndLoad(string scenePath)
    {
        BlackScreen.blocksRaycasts = true;
        yield return FadeHelper.Fade(BlackScreen, 1f, fadeTime);
        yield return SceneManager.LoadSceneAsync(scenePath);
        do yield return null;
        while (Time.unscaledDeltaTime > 0.05f);
        yield return FadeHelper.Fade(BlackScreen, 0f, fadeTime);
        BlackScreen.blocksRaycasts = false;
    }
}