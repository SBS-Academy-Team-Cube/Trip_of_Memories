using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour
{
    private StoryData CurrentStoryData = null;

    [Header("Skip Input Action Reference")]
    [SerializeField] private InputActionReference SkipAction;
    [SerializeField] private InputActionReference PlayerJumpAction;
    [Header("UI Object Reference")]
    [SerializeField] private GameObject StoryPanel;
    public Action<FStoryContext, int> OnStoryChanged;
    public Action<string> OnSkipRequested;
    public Action OnStoryEnd;
    private int Index = 0;
    private bool IsTyping = false;
    private bool IsSkipSubscribed = false;
    void OnDisable()
    {
        UnsubscribeSkip();
    }
    private void Start()
    {
        StoryPanel?.SetActive(false);
    }
    public void ShowStory(StoryData Data)
    {
        CurrentStoryData = Data;
        StoryPanel?.SetActive(true);
        Index = 0;
        SubscribeSkip();
        SkipAction?.action.Enable();
        PlayerJumpAction?.action.Disable();
        ShowCurrent();
    }
    private void EndStory()
    {
        UnsubscribeSkip();
        SkipAction?.action.Disable();
        PlayerJumpAction?.action.Enable();
        OnStoryEnd?.Invoke();
        StoryPanel?.SetActive(false);
    }

    private void SubscribeSkip()
    {
        if (IsSkipSubscribed || SkipAction == null)
        {
            return;
        }

        SkipAction.action.performed += OnSkip;
        IsSkipSubscribed = true;
    }

    private void UnsubscribeSkip()
    {
        if (!IsSkipSubscribed || SkipAction == null)
        {
            return;
        }

        SkipAction.action.performed -= OnSkip;
        IsSkipSubscribed = false;
    }

    void ShowCurrent()
    {
        if (Index >= CurrentStoryData.Count)
        {
            EndStory();
            return;
        }
        IsTyping = true;
        OnStoryChanged?.Invoke(CurrentStoryData.Get(Index), Index);
    }
    public void NotifyTypingFinished()
    {
        IsTyping = false;
    }
    private void OnSkip(InputAction.CallbackContext context)
    {
        if (!CurrentStoryData)
        {
            return;
        }
        if (IsTyping)
        {
            OnSkipRequested?.Invoke(CurrentStoryData.Get(Index).Text);
            return;
        }
        Index++;
        ShowCurrent();
    }
}
