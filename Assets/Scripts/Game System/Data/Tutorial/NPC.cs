using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    [SerializeField] private string NPCId;
    [SerializeField] private StoryManager Story;
    [SerializeField] private WaterGameManager MiniGame;
    private bool Triggered = false;
    public void ResetTrigger()
    {
        Triggered = false;
    }
    private void OnDisable()
    {
        Triggered = false;
    }
    private void OnTriggerEnter(Collider Other)
    {
        if (Triggered)
            return;

        if (!Other.CompareTag("Player"))
            return;
        Triggered = true;
        if (SaveManager.Instance && SaveManager.Instance.CurrentLevelProgress != null)
        {
            var Progress = SaveManager.Instance.CurrentLevelProgress;

            if (Progress.HasCompletedInteraction(NPCId))
            {
                if (Progress.HasClearedMiniGame(MiniGame.ID))
                {
                    return;
                }
                else
                {
                    Story.ShowStory(1);
                }
            }
            else
            {
                SaveManager.Instance.CurrentLevelProgress.AddCompletedInteraction(NPCId);
                Story.ShowStory(0);
            }
        }
        Story.OnStoryEnd += HandleConnectStroyMiniGame;
    }
    private void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            Triggered = false;
        }
    }
    private void HandleConnectStroyMiniGame()
    {
        MiniGame.Play();
        MiniGame.OnClear += HandleClear;
        MiniGame.OnFail += HandleFail;
        Story.OnStoryEnd -= HandleConnectStroyMiniGame;
    }
    private void HandleClear()
    {
        MiniGame.OnClear -= HandleClear;
        Story.ShowStory(2);
    }
    private void HandleFail()
    {
        MiniGame.OnFail -= HandleFail;
        Story.ShowStory(3);
    }
}