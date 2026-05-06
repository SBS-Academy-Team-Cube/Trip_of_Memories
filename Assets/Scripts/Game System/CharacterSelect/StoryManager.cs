using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public struct DialogueContext
{
    public int Index;
    public string Text;
}

public class StoryManager : MonoBehaviour
{
    [SerializeField] private List<DialogueData> Dialogues;
    private DialogueData CurrentDialogue = null;
    [SerializeField] private InputActionReference SkipAction;
    [SerializeField] private InputActionReference PlayerJumpAction;
    public Action<DialogueContext> OnDialogueChanged;
    public Action<string> OnSkipRequested;
    public Action OnDialogueEnd;
    public UnityEvent OnMiniGameStart;
    [SerializeField] private GameObject StoryPanel;

    private int Index = 0;
    private bool IsTyping = false;
    private bool IsPlayed = false;
    void OnEnable()
    {
        SkipAction.action.performed += OnSkip;
    }
    void OnDisable()
    {
        SkipAction.action.performed -= OnSkip;
    }
    public void ShowStory(int Index)
    {
        CurrentDialogue = Dialogues[Index];
        StoryPanel?.SetActive(true);
        StartDialogue();
    }
    public void StartDialogue()
    {
        Index = 0;
        ShowCurrent();
        PlayerJumpAction?.action.Disable();
    }
    void ShowCurrent()
    {
        if (Index >= CurrentDialogue.Dialogues.Count)
        {
            return;
        }
        string text = CurrentDialogue.GetText(Index);
        IsTyping = true;
        OnDialogueChanged?.Invoke(new DialogueContext
        {
            Index = Index,
            Text = CurrentDialogue.GetText(Index)
        });
    }
    public void NotifyTypingFinished()
    {
        IsTyping = false;
    }
    private void OnSkip(InputAction.CallbackContext context)
    {
        Debug.Log("OnSkip!");
        if (!CurrentDialogue)
        {
            return;
        }
        if (IsTyping)
        {
            OnSkipRequested?.Invoke(CurrentDialogue.GetText(Index));
        }
        else
        {
            Index++;
            if (Index < CurrentDialogue.Dialogues.Count)
            {
                ShowCurrent();
            }
            else
            {
                OnDialogueEnd?.Invoke();
                PlayerJumpAction?.action.Enable();
                StoryPanel?.SetActive(false);
                if (!IsPlayed)
                {
                    OnMiniGameStart?.Invoke();
                    IsPlayed = true;
                }
            }
        }
    }
}