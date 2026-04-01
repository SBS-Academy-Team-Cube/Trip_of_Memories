using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private DialogueData Dialogues;
    [SerializeField] private TypewriterEffect Effect;
    [SerializeField] private TypewriterPlayer DialoguePlayer;
    [SerializeField] private InputActionReference SkipAction;

    [SerializeField] private CharacterSelectController Controller;
    [SerializeField] private GameObject[] StoryObjects;
    [SerializeField] private GameObject[] SelectObjects;
    private int Index = 0;

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
        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlayBGM(0);
        }
    }

    void StartDialogue()
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

        DialoguePlayer.Play(Dialogues.GetText(Index), Effect);
    }

    private void OnSkip(InputAction.CallbackContext context)
    {
        if (DialoguePlayer.IsTyping)
        {
            DialoguePlayer.Skip(Dialogues.GetText(Index));
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
                EndDialogue();
            }
        }
    }
    void EndDialogue()
    {
        foreach (var Obj in StoryObjects)
        {
            Obj.SetActive(false);
        }
        foreach (var Obj in SelectObjects)
        {
            Obj.SetActive(true);
        }
        DialoguePlayer.enabled = false;
        Controller.enabled = true;
        enabled = false;
    }
}