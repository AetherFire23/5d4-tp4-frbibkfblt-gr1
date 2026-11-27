using System;
using System.Collections;
using UI_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const float fadeTime = 0.5f;

    CanvasGroup blackScreen;
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }

    void Awake()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        blackScreen = gameObject.AddComponent<CanvasGroup>();
        blackScreen.alpha = 0f;
        blackScreen.blocksRaycasts = false;

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

        SetState(scene.name == "MainScene" ? GameState.Playing : GameState.Menu);
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

    IEnumerator FadeAndLoad(string scenePath)
    {
        blackScreen.blocksRaycasts = true;
        yield return FadeHelper.Fade(blackScreen, 1f, fadeTime);
        yield return SceneManager.LoadSceneAsync(scenePath);
        do yield return null;
        while (Time.unscaledDeltaTime > 0.05f);
        yield return FadeHelper.Fade(blackScreen, 0f, fadeTime);
        blackScreen.blocksRaycasts = false;
    }
}