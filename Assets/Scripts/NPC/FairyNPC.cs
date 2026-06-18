using UnityEngine;

public class FairyNPC : MonoBehaviour
{
    [SerializeField] private string NPCId;
    [SerializeField] private string SpeakerName;
    [SerializeField] private int MemoryRecoveryAmount;
    [SerializeField] private StoryManager StoryManager;
    [SerializeField] private NPCAnimation Animation;

    [Header("Story Data")]
    [SerializeField] private StoryData FirstTime;
    [SerializeField] private Collider MyTrigger;
    [SerializeField] private Trigger Trigger;
    private bool bInteracted = false;
    private PlayerInputController PlayerInputComponent = null;
    private void OnEnable()
    {
        if (SaveManager.Instance)
        {
            bInteracted = SaveManager.Instance.HasCompletedInteraction(NPCId);
        }
    }
    private void OnDisable()
    {

    }
    void OnTriggerEnter(Collider Other)
    {
        if (bInteracted)
        {
            return;
        }
        if (Other.CompareTag("Player"))
        {
            if (StoryManager)
            {
                Trigger.OnTrigger();

                if (Other.TryGetComponent(out PlayerInputComponent))
                {
                    PlayerInputComponent.LockInput();
                }
                StoryManager.OnStoryEnd += UnlockPlayerInput;
                StoryManager.OnStoryChanged += TryTalk;
                StoryManager.ShowStory(FirstTime);
            }
        }
    }
    private void UnlockPlayerInput()
    {
        StoryManager.OnStoryEnd -= UnlockPlayerInput;
        if (SaveManager.Instance)
        {
            bInteracted = SaveManager.Instance.TryCompleteInteraction(NPCId, MemoryRecoveryAmount);
        }
        if (PlayerInputComponent == null)
        {
            return;
        }
        PlayerInputComponent.UnLockInput();
        PlayerInputComponent = null;
        Trigger.OnTrigger();
    }
    private void TryTalk(FStoryContext Context, int Index)
    {
        if (Animation && Context.Speaker.Equals(SpeakerName))
        {
            Animation.SetTalking();
        }
    }
}
