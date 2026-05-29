using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct DialogueContext
{
    public int Index;
    public string Text;
    public string Speaker;
}
public class StoryManager : MonoBehaviour
{
    [Header("Story Data")]
    [SerializeField] private List<DialogueData> StoryDialogues;
    private DialogueData CurrentDialogue = null;

    [Header("Skip Input Action Reference")]
    [SerializeField] private InputActionReference SkipAction;
    [SerializeField] private InputActionReference PlayerJumpAction;
    [Header("UI Object Reference")]
    [SerializeField] private GameObject StoryPanel;
    public Action<DialogueContext> OnDialogueChanged;
    public Action<string> OnSkipRequested;
    public Action OnStoryEnd;
    private int Index = 0;
    private bool IsTyping = false;
    void OnEnable()
    {
        SkipAction.action.performed += OnSkip;
    }
    void OnDisable()
    {
        SkipAction.action.performed -= OnSkip;
    }
    private void Start() 
    {
        StoryPanel?.SetActive(false);
    }
    public void ShowStory(int Index)
    {
        CurrentDialogue = StoryDialogues[Index];
        StoryPanel?.SetActive(true);
        this.Index = 0;
        SkipAction?.action.Enable();
        PlayerJumpAction?.action.Disable();
        ShowCurrent();
    }
    private void EndStory()
    {
        SkipAction?.action.Disable();
        PlayerJumpAction?.action.Enable();
        OnStoryEnd?.Invoke();
        StoryPanel?.SetActive(false);
    }
    void ShowCurrent()
    {
        if (Index >= CurrentDialogue.Dialogues.Count)
        {
            EndStory();
            return;
        }
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
        if (!CurrentDialogue)
        {
            return;
        }
        if (IsTyping)
        {
            OnSkipRequested?.Invoke(CurrentDialogue.GetText(Index));
            return;
        }
        Index++;
        ShowCurrent();
    }
}