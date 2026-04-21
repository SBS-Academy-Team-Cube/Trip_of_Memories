using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class LevelManager : MonoBehaviour
{
    public List<UnityEvent> LevelEvents;
    private int DoorConditionIndex = 0;
    [SerializeField] private int RequiredConditionCounts = 2;


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
}
