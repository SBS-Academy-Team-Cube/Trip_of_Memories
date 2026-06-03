using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class CutsceneManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayableDirector Director;
    [SerializeField] private CutsceneTrigger[] Triggers;
    
    private PlayerMovement PlayerMovement = null;
    private PlayerAnimation PlayerAnimation = null;

    public void SetPlayerMovement(GameObject Player)
    {
        Player.TryGetComponent(out PlayerMovement);
        Player.TryGetComponent(out PlayerAnimation);
    }
    private void OnEnable() 
    {
        if (Director != null)
        {
            Director.stopped += OnCutsceneEnd;
        }
        foreach (var Trigger in Triggers)
        {
            Trigger.OnCutSceneTriggered += HandleCutScenePlay;
        }
    }
    private void OnDisable() 
    {
        if (Director != null)
        {
            Director.stopped -= OnCutsceneEnd;
        }
        foreach (var Trigger in Triggers)
        {
            Trigger.OnCutSceneTriggered -= HandleCutScenePlay;
        }
    }
    public void HandleCutScenePlay(TimelineAsset Data)
    {
        LockPlayer();
        Director.Play(Data);
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        UnlockPlayer();
    }

    private void LockPlayer()
    {
        if(PlayerMovement != null)
        {
            PlayerMovement.enabled = false;
        }   
        if(PlayerAnimation != null)
        {
            PlayerAnimation.SetIsMoving(false);
        }
    }
    private void UnlockPlayer()
    {
        if(PlayerMovement != null)
        {
            PlayerMovement.enabled = true;
        }
    }
}