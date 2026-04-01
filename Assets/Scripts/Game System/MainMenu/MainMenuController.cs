using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject InitialPanel;
    [SerializeField] private List<GameObject> Panels;
    [SerializeField] private InputActionReference CancelAction;

    private readonly Stack<GameObject> PanelHistory = new();

    void Start()
    {
        OpenPanel(InitialPanel, false);
    }

    void OnEnable()
    {
        CancelAction.action.performed += OnCancel;
        CancelAction.action.Enable();
    }
    void OnDisable()
    {
        CancelAction.action.performed -= OnCancel;
        CancelAction.action.Disable();
    }
    private void OnCancel(InputAction.CallbackContext Context)
    {
        HandleBack();
    }
    void Update()
    {
    }
    public void OpenPanel(GameObject Target, bool SaveHistory = true)
    {
        GameObject Current = null;
        foreach (var Panel in Panels)
        {
            if (Panel.activeSelf)
            {
                Current = Panel;
                Panel.SetActive(false);
            }
        }
        if (SaveHistory && Current != null)
        {
            PanelHistory.Push(Current);
        }
        Target.SetActive(true);
    }
    public void OpenPanel(GameObject Target)
    {
        OpenPanel(Target, true);
    }
    public void HandleBack()
    {
        if (PanelHistory.Count > 0)
        {
            var Prev = PanelHistory.Pop();
            OpenPanel(Prev, false);
        }
        else
        {
            Debug.Log("Panel Stack is Empty");
        }
    }

    public void OnQuitGame()
    {
        if (GameDirector.Instance != null)
        {
            GameDirector.Instance.QuitGame();
        }
        else
        {
            Debug.Log("Can't Find GameDirector Object");
        }
    }
    public void OnButtonClicked()
    {
        UIEventBus.OnAnyButtonClicked?.Invoke();
    }
    public void OnNewGame()
    {
        if(SaveManager.Instance)
        {
            SaveManager.Instance.ResetSave();
        }
        if(GameDirector.Instance)
        {
            GameDirector.Instance.LoadScene(1);
        }
    }
    public void OnLoadGame()
    {
        if(SaveManager.Instance.Data == null)
        {
            SaveManager.Instance.Load();
        }
        if(SaveManager.Instance.Data.StageIndex > 0)
        {
            GameDirector.Instance.LoadScene(SaveManager.Instance.Data.StageIndex);
        }
    }
}