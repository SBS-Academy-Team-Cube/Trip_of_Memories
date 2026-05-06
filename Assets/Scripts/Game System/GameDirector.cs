using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;
public enum GameState
{
    None,
    Loading,
    CharacterSelect,
    InGame
}
public class GameDirector : Singleton<GameDirector>
{
    public SceneId NextSceneID { get; private set; }
    public SceneId CurrentSceneID { get; private set; }
    private readonly SceneId LoadingSceneID = SceneId.Loading;
    public GameState CurrentState { get; private set; } = GameState.None;
    public Action<GameState> OnGameStateChanged;
    public IrisController Iris;
    public InputActionReference SlowModeAction;
    private bool bSlowMode;
    protected override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SlowModeAction)
        {
            SlowModeAction.action.performed += SlowMode;
            SlowModeAction.action.Enable();
        }
    }
    private void SlowMode(InputAction.CallbackContext Context)
    {
        bSlowMode = !bSlowMode;
        Time.timeScale = bSlowMode ? .25f : 1.0f;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (SlowModeAction)
        {
            SlowModeAction.action.performed -= SlowMode;
            SlowModeAction.action.Disable();
        }
    }
    public void LoadScene(SceneId ID)
    {
        NextSceneID = ID;
        SceneManager.LoadScene(SceneTable.GetSceneName(LoadingSceneID));
    }
    public void LoadSceneWithoutLoading(SceneId ID)
    {
        NextSceneID = ID;
        SceneManager.LoadScene(SceneTable.GetSceneName(ID));
    }
    public AsyncOperation AsyncLoading()
    {
        return SceneManager.LoadSceneAsync(SceneTable.GetSceneName(NextSceneID));
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneTable.GetSceneName(SceneId.Loading))
        {
            SetState(GameState.Loading);
            ShowMouseCursor(true);
            return;
        }

        CurrentSceneID = NextSceneID;
        NextSceneID = SceneId.NULL;

        GameState state = SceneTable.GetGameState(CurrentSceneID);
        SetState(state);
        ShowMouseCursor(state != GameState.InGame);
    }

    public void SetState(GameState NewState)
    {
        if (CurrentState == NewState)
        {
            return;
        }
        CurrentState = NewState;
        OnGameStateChanged?.Invoke(CurrentState);
    }
    private void ShowMouseCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}