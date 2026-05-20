using UnityEngine;
using TMPro;
public class DialoguePresenter : MonoBehaviour
{
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private TypewriterPlayer player;
    [SerializeField] private TMP_Text SpeakerName;
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
        player.Play(Context.Text, effect);
    }
    void HandleTypingFinished()
    {
        storyManager.NotifyTypingFinished();
    }
}