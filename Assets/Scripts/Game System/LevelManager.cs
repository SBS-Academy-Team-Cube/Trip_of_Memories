using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class LevelManager : MonoBehaviour
{
    public List<UnityEvent> LevelEvents;
    private int DoorConditionIndex = 0;
    [SerializeField] private int RequiredConditionCounts = 2;
    [SerializeField] private int NextSceneIndex = 0;
    
    [SerializeField] private IrisController TransitionController;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip LevelBGM;
    
    private void Start()
    {
        if (AudioManager.Instance && LevelBGM)
        {
            AudioManager.Instance.PlayBGM(LevelBGM);
        }
    }
    public void OnConditionMet()
    {
        DoorConditionIndex++;
        if (DoorConditionIndex == RequiredConditionCounts)
        {
            LevelEvents[0]?.Invoke();
        }
    }
    
    public void LoadNextLevel()
    {
        if(TransitionController)
        {
            // TransitionController.StartTransition();
        }
    }

}
