using UnityEngine;
using UnityEngine.InputSystem;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private string InteractionStoryId;
    [SerializeField] private StoryManager StoryManager = null;
    [SerializeField] private StoryData StoryData = null;
    [SerializeField] private Collider MyTrigger;
    private bool IsTriggered = false;
    private PlayerInputController PlayerInputComponent = null;
    void Awake()
    {
        if (MyTrigger == null)
        {
            TryGetComponent(out MyTrigger);
        }
    }
    private void OnEnable()
    {
        if (SaveManager.Instance)
        {
            IsTriggered = SaveManager.Instance.HasCompletedInteraction(InteractionStoryId);
        }
        if (MyTrigger)
        {
            MyTrigger.enabled = !IsTriggered;
        }
    }
    private void OnTriggerEnter(Collider Other)
    {
        if (IsTriggered)
        {
            return;
        }
        if (Other.CompareTag("Player") && SaveManager.Instance && StoryData && StoryManager)
        {
            IsTriggered = true;
            if (Other.TryGetComponent(out PlayerInputComponent))
            {
                PlayerInputComponent.LockInput();
            }
            StoryManager.OnStoryEnd += HandleStoryEnd;
            StoryManager.ShowStory(StoryData);
        }
    }
    private void HandleStoryEnd()
    {
        StoryManager.OnStoryEnd -= HandleStoryEnd;
        SaveManager.Instance.TryCompleteInteraction(InteractionStoryId);
        if (PlayerInputComponent)
        {
            PlayerInputComponent.UnLockInput();
        }
    }
}