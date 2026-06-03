using UnityEngine;
using UnityEngine.Timeline;
using System;

public class CutsceneTrigger : MonoBehaviour
{
    public event Action<TimelineAsset > OnCutSceneTriggered;
    [SerializeField] private TimelineAsset CutSceneData;
    private bool IsTriggered = false;
    private void OnTriggerEnter(Collider Other) 
    {
        if(IsTriggered)
        {
            return;
        }
        if(Other.CompareTag("Player"))
        {
            IsTriggered = true;
            OnCutSceneTriggered?.Invoke(CutSceneData);
        }
    }
}