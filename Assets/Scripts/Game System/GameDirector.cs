using UnityEngine;
using UnityEngine.SceneManagement;

// Singleton pattern
// Ensures that only one GameDirector instance exists and provides global access to it
public class GameDirector : Singleton<GameDirector>
{
    public string NextScene { get; private set; }
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
        NextScene = SceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadSceneWithOutLoading(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    
    private void Start()
    {
        ShowMouseCursor(false);
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
        if (scene.name.Equals("TestScene_LJW"))
        {
            ShowMouseCursor(false);
        }
    }
}