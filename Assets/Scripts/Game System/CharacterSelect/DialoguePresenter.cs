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
        storyManager.OnStoryChanged += HandleStoryChanged;
        storyManager.OnSkipRequested += HandleSkipRequested;

        player.OnTypingFinished += HandleTypingFinished;
    }
    private void OnDisable()
    {
        storyManager.OnStoryChanged -= HandleStoryChanged;
        storyManager.OnSkipRequested -= HandleSkipRequested;

        player.OnTypingFinished -= HandleTypingFinished;
    }
    void HandleSkipRequested(string fullText)
    {
        player.Skip(fullText);
    }
    void HandleStoryChanged(FStoryContext Context, int Index)
    {
        SpeakerName.text = Context.Speaker;
        if (Context.NoEffect)
        {
            player.Play(Context.Text);
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlaySFX(Context.SFX);
            }
        }
        else
        {
            player.Play(Context.Text, effect);
        }
    }
    void HandleTypingFinished()
    {
        storyManager.NotifyTypingFinished();
    }
}