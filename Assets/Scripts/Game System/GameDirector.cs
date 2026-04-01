using UnityEngine;
using UnityEngine.SceneManagement;

// Singleton pattern
// Ensures that only one GameDirector instance exists and provides global access to it
public class GameDirector : Singleton<GameDirector>
{
    public string NextSceneName { get; private set; }
    public int NextSceneIndex { get; private set; }
    public bool bUseSceneName { get; private set; } = true;
    protected override void Awake()
    {
        base.Awake();

        // GameManager Initialize
    }
    private void ShowMouseCursor(bool bShowing)
    {
        Cursor.lockState = bShowing ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = bShowing;
    }
    public void LoadScene(string SceneName)
    {
        NextSceneName = SceneName;
        bUseSceneName = true;
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadScene(int SceneIndex)
    {
        NextSceneIndex = SceneIndex;
        bUseSceneName = false;
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadSceneWithOutLoading(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void LoadSceneWithOutLoading(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 테스트용 하드코딩
        if (scene.name.Equals("TestScene_LJW"))
        {
            ShowMouseCursor(false);
        }
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