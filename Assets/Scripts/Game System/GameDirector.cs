using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameState
{
    None,
    Loading,
    CharacterSelect,
    InGame
}
public class GameDirector : Singleton<GameDirector>
{
    public string NextSceneName { get; private set; }
    public int NextSceneIndex { get; private set; }
    public bool bUseSceneName { get; private set; } = true;
    public GameState CurrentState { get; private set; } = GameState.None;
    public Action<GameState> OnGameStateChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        NextSceneName = sceneName;
        bUseSceneName = true;

        SetState(GameState.Loading);
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadScene(int sceneIndex)
    {
        NextSceneIndex = sceneIndex;
        bUseSceneName = false;

        SetState(GameState.Loading);
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadSceneWithoutLoading(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithoutLoading(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "CharacterSelectScene":
                SetState(GameState.CharacterSelect);
                ShowMouseCursor(true);
                break;

            case "GameScene":
                SetState(GameState.InGame);
                ShowMouseCursor(false);
                break;

            case "LoadingScene":
                SetState(GameState.Loading);
                break;

            default:
                if (scene.name.StartsWith("Level"))
                {
                    ShowMouseCursor(false);
                    SetState(GameState.InGame); // 필요에 맞게 상태 지정
                }
                else
                {
                    ShowMouseCursor(true);
                    SetState(GameState.None);
                }
                break;
        }
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
        {
            return;
        }
        CurrentState = newState;
        Debug.Log($"[GameDirector] State Changed → {newState}");

        OnGameStateChanged?.Invoke(newState);
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