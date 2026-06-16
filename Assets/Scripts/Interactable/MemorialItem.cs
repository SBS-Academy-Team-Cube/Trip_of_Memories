using UnityEngine;

public class MemorialItem : MonoBehaviour
{
    [Header("Required Component")]
    [SerializeField] private Collider MyTrigger;
    [SerializeField] private StoryManager StoryManager;
    [SerializeField] private StoryData Story;
    [SerializeField] private Trigger Trigger;
    [SerializeField] private string ID;
    [SerializeField] private int MemoryRecoverAmount;
    private PlayerInputController PlayerInputComponent = null;
    bool IsCollected = false;
    private void Awake()
    {
        if (MyTrigger == null)
        {
            TryGetComponent(out MyTrigger);
        }
        if (SaveManager.Instance != null)
        {
            IsCollected = SaveManager.Instance.HasCollectedMemoryItem(ID);
        }
        if (MyTrigger)
        {
            MyTrigger.enabled = !IsCollected;
        }
    }
    void OnTriggerEnter(Collider Other)
    {
        if (IsCollected)
        {
            return;
        }
        if (Other.CompareTag("Player") && StoryManager && Story)
        {
            if (Other.TryGetComponent(out PlayerInputComponent))
            {
                PlayerInputComponent.LockInput();
            }
            StoryManager.ShowStory(Story);
            StoryManager.OnStoryEnd += OnStoryEnd;

            if (Trigger)
            {
                Trigger.OnTrigger();
            }
        }
    }
    private void OnStoryEnd()
    {
        StoryManager.OnStoryEnd -= OnStoryEnd;
        PlayerInputComponent?.UnLockInput();
        IsCollected = true;
        if (SaveManager.Instance)
        {
            SaveManager.Instance.TryCollectMemoryItem(ID, MemoryRecoverAmount);
        }

        if (Trigger)
        {
            Trigger.OnTrigger();
        }
    }
}
