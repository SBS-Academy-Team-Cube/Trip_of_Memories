using UnityEngine;

public class BatNPC : MonoBehaviour
{
    [SerializeField] private string NPCId;
    [SerializeField] private string SpeakerName;
    [SerializeField] private int MemoryRecoveryAmount;
    [SerializeField] private StoryManager StoryManager;
    [SerializeField] private NPCAnimation Animation;

    [SerializeField] private Collider MyTrigger;
    [SerializeField] private Trigger Trigger;
    [Header("Story Data")]
    [SerializeField] private StoryData FirstTime;

    private bool bInteracted = false;
    private PlayerInputController PlayerInputComponent = null;
    
    private void OnEnable()
    {
        if (SaveManager.Instance)
        {
            bInteracted = SaveManager.Instance.HasCompletedInteraction(NPCId);
        }
    }
    private void TryTalk(FStoryContext Context, int Index)
    {
        if (Animation && Context.Speaker.Equals(SpeakerName))
        {
            Animation.SetTalking();
        }
    }
}
