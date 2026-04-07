using System;
using UnityEngine;
using UnityEngine.InputSystem;

public struct DialogueContext
{
    public int Index;
    public string Text;
}

public class StoryManager : MonoBehaviour
{
    [SerializeField] private DialogueData Dialogues;
    [SerializeField] private InputActionReference SkipAction;
    public Action<DialogueContext> OnDialogueChanged;
    public Action<string> OnSkipRequested;
    public Action OnDialogueEnd;

    private int Index = 0;
    private bool IsTyping = false;

    void OnEnable()
    {
        SkipAction.action.performed += OnSkip;
        SkipAction.action.Enable();
    }

    void OnDisable()
    {
        SkipAction.action.performed -= OnSkip;
        SkipAction.action.Disable();
    }
    void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        Index = 0;
        ShowCurrent();
    }
    void ShowCurrent()
    {
        if (Index >= Dialogues.Dialogues.Count)
        {
            return;
        }
        string text = Dialogues.GetText(Index);
        IsTyping = true;
        OnDialogueChanged?.Invoke(new DialogueContext
        {
            Index = Index,
            Text = Dialogues.GetText(Index)
        });
    }

    public void NotifyTypingFinished()
    {
        IsTyping = false;
    }

    private void OnSkip(InputAction.CallbackContext context)
    {
        if (IsTyping)
        {
            OnSkipRequested?.Invoke(Dialogues.GetText(Index));
        }
        else
        {
            Index++;
            if (Index < Dialogues.Dialogues.Count)
            {
                ShowCurrent();
            }
            else
            {
                OnDialogueEnd?.Invoke();
            }
        }
    }
}