using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;
public enum GameState
{
    None,
    Loading,
    CharacterSelect,
    InGame,
    MainMenu,
}
public class GameDirector : Singleton<GameDirector>
{
    [Header("Test Option")]
    public bool bTesting = false;
    public SceneId NextSceneID { get; private set; }
    public SceneId CurrentSceneID { get; private set; }
    private readonly SceneId LoadingSceneID = SceneId.Loading;
    public GameState CurrentState { get; private set; } = GameState.None;
    public Action<GameState> OnGameStateChanged;

    [Header("Director's Components")]
    public IrisController Iris { get; private set; } = null;
    [SerializeField] private MouseCursorManager CursorManager = null;
    [SerializeField] private InputActionReference SlowModeAction;
    [SerializeField] private InputActionReference PauseAction;
    private bool bPaused = false;
    public bool IsPaused => bPaused;
    public event Action<bool> OnPaused;
    private bool bSlowMode;

    void Start()
    {
        if (bTesting)
        {
            ShowMouseCursor(false);
            CurrentState = GameState.InGame;
        }
    }
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
    public void EnablePauseAction(bool bEnable)
    {
        if (PauseAction)
        {
            if (bEnable)
            {
                PauseAction.action.Enable();
            }
            else
            {
                PauseAction.action.Disable();
            }
        }
    }
    private void PauseGame(InputAction.CallbackContext Context)
    {
        if (CurrentState != GameState.InGame)
        {
            return;
        }
        SetPaused(!bPaused);
    }
    public void SetPaused(bool Paused)
    {
        if (bPaused == Paused)
        {
            return;
        }

        bPaused = Paused;
        ApplyTimeScale();
        ShowMouseCursor(bPaused);
        OnPaused?.Invoke(bPaused);
    }
    public void ContinueGame()
    {
        SetPaused(false);
    }

    private void SlowMode(InputAction.CallbackContext Context)
    {
        if (CurrentState != GameState.InGame || bPaused)
        {
            return;
        }

        bSlowMode = !bSlowMode;
        ApplyTimeScale();
    }
    private void ApplyTimeScale()
    {
        Time.timeScale = bPaused ? 0.0f : bSlowMode ? .25f : 1.0f;
    }
    private void ResetTimeControl()
    {
        bool WasPaused = bPaused;
        bPaused = false;
        bSlowMode = false;
        Time.timeScale = 1.0f;
        if (WasPaused)
        {
            OnPaused?.Invoke(false);
        }
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
        ResetTimeControl();
        NextSceneID = ID;
        SceneManager.LoadScene(SceneTable.GetSceneName(LoadingSceneID));
    }
    public void LoadSceneWithoutLoading(SceneId ID)
    {
        ResetTimeControl();
        NextSceneID = ID;
        SceneManager.LoadScene(SceneTable.GetSceneName(ID));
    }
    public AsyncOperation AsyncLoading()
    {
        return SceneManager.LoadSceneAsync(SceneTable.GetSceneName(NextSceneID));
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetTimeControl();

        if (SceneTable.IsLoadingScene(scene))
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
    public void ShowMouseCursor(bool bShow)
    {
        if (CursorManager != null)
        {
            CursorManager.SetVisible(bShow);
            return;
        }
        // Cursor.lockState = bShow ? CursorLockMode.None : CursorLockMode.Locked;
        // Cursor.visible = bShow;
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
