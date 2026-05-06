using UnityEngine;

public class DialoguePresenter : MonoBehaviour
{
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private TypewriterPlayer player;
    [SerializeField] private TypewriterEffect effect;
    private void OnEnable()
    {
        storyManager.OnDialogueChanged += HandleDialogueChanged;
        storyManager.OnSkipRequested += HandleSkipRequested;

        player.OnTypingFinished += HandleTypingFinished;
    }

    private void OnDisable()
    {
        storyManager.OnDialogueChanged -= HandleDialogueChanged;
        storyManager.OnSkipRequested -= HandleSkipRequested;

        player.OnTypingFinished -= HandleTypingFinished;
    }
    void HandleSkipRequested(string fullText)
    {
        player.Skip(fullText);
    }
    void HandleDialogueChanged(DialogueContext Context)
    {
        Debug.Log("HandleDialougeChanged");
        player.Play(Context.Text, effect);
    }
    void HandleTypingFinished()
    {
        storyManager.NotifyTypingFinished();
    }
}