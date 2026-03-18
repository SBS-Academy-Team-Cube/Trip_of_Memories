using UnityEngine;
using UnityEngine.InputSystem;


// Singleton pattern
// Ensures that only one GameDirector instance exists and provides global access to it
public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    public UIManager UI { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        UI = GetComponentInChildren<UIManager>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if(!UI)
        {
            Debug.Log("Can't Find UI Manager in Game Director");
        }
        ShowMouseCursor(false);
    }
    private void ShowMouseCursor(bool bShowing)
    {
        Cursor.lockState = bShowing ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = bShowing;
    }

    public void ShowMainMenu(bool bShowing)
    {
        if(UI)
        {
            UI.ShowMainMenu(bShowing);
            ShowMouseCursor(bShowing);
        }
    }
}