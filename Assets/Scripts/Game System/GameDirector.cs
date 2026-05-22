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
    [SerializeField] private InputActionReference SlowModeAction;
    [SerializeField] private InputActionReference PauseAction;
    private bool bPaused = false;
    public event Action<bool> OnPaused;
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
        if (PauseAction)
        {
            PauseAction.action.performed += PauseGame;
            PauseAction.action.Enable();
        }
    }
    private void PauseGame(InputAction.CallbackContext Context)
    {
        bPaused = !bPaused;
        Time.timeScale = bPaused ? 0.0f : 1.0f;
        OnPaused?.Invoke(bPaused);
    }
    public void ContinueGame()
    {
        if (!bPaused)
        {
            return;
        }
        bPaused = false;
        Time.timeScale = 1.0f;
        OnPaused?.Invoke(bPaused);
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
        if (PauseAction)
        {
            PauseAction.action.performed -= PauseGame;
            PauseAction.action.Disable();
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
    public void ShowMouseCursor(bool show)
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