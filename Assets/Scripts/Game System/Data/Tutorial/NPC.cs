using UnityEngine;
public class NPC : MonoBehaviour
{
    [SerializeField] private string NPCId;
    [SerializeField] private StoryManager StoryManager;
    [SerializeField] private NPCAnimation Animation;
    [SerializeField] private MiniGameBase MiniGame;

    [Header("Story Data")]
    [SerializeField] private StoryData FirstTime;
    [SerializeField] private StoryData ReTry;
    [SerializeField] private StoryData Clear;
    [SerializeField] private StoryData Fail;

    [SerializeField] private Collider Trigger;
    private bool Triggered = false;
    private bool IsStoryChangedSubscribed = false;
    private bool IsConnectStoryEndSubscribed = false;
    private bool IsCleanupStoryEndSubscribed = false;
    public void ResetTrigger()
    {
        Triggered = false;
    }
    private void OnEnable()
    {
        if (SaveManager.Instance != null)
        {
            if (SaveManager.Instance.HasCompletedInteraction(NPCId) && MiniGame.HasCleared())
            {
                Trigger.enabled = false;
            }
        }
    }
    private void OnDisable()
    {
        Triggered = false;
        UnsubscribeStoryEvents();
        if (MiniGame != null)
        {
            MiniGame.OnClear -= HandleClear;
            MiniGame.OnFail -= HandleFail;
        }
    }
    private void OnTriggerEnter(Collider Other)
    {
        if (Triggered || !Other.CompareTag("Player") || SaveManager.Instance == null || MiniGame.HasCleared())
        {
            return;
        }
        Triggered = true;

        SubscribeStoryEvents(true);

        if (SaveManager.Instance.HasCompletedInteraction(NPCId))
        {
            StoryManager.ShowStory(ReTry);
        }
        else
        {
            SaveManager.Instance.TryCompleteInteraction(NPCId);
            StoryManager.ShowStory(FirstTime);
        }

    }
    private void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            Triggered = false;
        }
    }
    private void TryTalk(FStoryContext Context, int Index)
    {
        if (Animation)
        {
            Animation.SetTalking();
        }
    }
    private void SubscribeStoryEvents(bool bConnectMiniGameOnEnd)
    {
        if (StoryManager == null)
        {
            return;
        }
        if (!IsStoryChangedSubscribed)
        {
            StoryManager.OnStoryChanged += TryTalk;
            IsStoryChangedSubscribed = true;
        }

        if (bConnectMiniGameOnEnd)
        {
            if (!IsConnectStoryEndSubscribed)
            {
                StoryManager.OnStoryEnd += HandleConnectStroyMiniGame;
                IsConnectStoryEndSubscribed = true;
            }

            if (IsCleanupStoryEndSubscribed)
            {
                StoryManager.OnStoryEnd -= HandleStoryEndCleanup;
                IsCleanupStoryEndSubscribed = false;
            }
        }
        else if (!IsCleanupStoryEndSubscribed)
        {
            StoryManager.OnStoryEnd += HandleStoryEndCleanup;
            IsCleanupStoryEndSubscribed = true;
        }
    }

    private void UnsubscribeStoryEvents()
    {
        if (StoryManager == null)
        {
            return;
        }

        if (IsStoryChangedSubscribed)
        {
            StoryManager.OnStoryChanged -= TryTalk;
            IsStoryChangedSubscribed = false;
        }

        if (IsConnectStoryEndSubscribed)
        {
            StoryManager.OnStoryEnd -= HandleConnectStroyMiniGame;
            IsConnectStoryEndSubscribed = false;
        }

        if (IsCleanupStoryEndSubscribed)
        {
            StoryManager.OnStoryEnd -= HandleStoryEndCleanup;
            IsCleanupStoryEndSubscribed = false;
        }
    }

    private void HandleStoryEndCleanup()
    {
        UnsubscribeStoryEvents();
    }

    private void HandleConnectStroyMiniGame()
    {
        MiniGame.Play();
        MiniGame.OnClear += HandleClear;
        MiniGame.OnFail += HandleFail;
        UnsubscribeStoryEvents();

    }
    private void HandleClear()
    {
        MiniGame.OnClear -= HandleClear;
        SubscribeStoryEvents(false);
        StoryManager.ShowStory(Clear);
    }

    private void HandleFail()
    {
        MiniGame.OnFail -= HandleFail;
        SubscribeStoryEvents(false);
        StoryManager.ShowStory(Fail);
    }
}
