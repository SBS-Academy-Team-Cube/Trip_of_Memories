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
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        UI = GetComponent<UIManager>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if(!UI)
        {
            Debug.Log("Can't Find UI Manager in Game Director");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}