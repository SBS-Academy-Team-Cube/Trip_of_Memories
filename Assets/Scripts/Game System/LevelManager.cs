using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class LevelManager : MonoBehaviour
{
    public List<UnityEvent> LevelEvents;
    private int DoorConditionIndex = 0;
    [SerializeField] private int RequiredConditionCounts = 2;

    public void OnConditionMet()
    {
        DoorConditionIndex++;
        if(DoorConditionIndex == RequiredConditionCounts)
        {
            LevelEvents[0]?.Invoke();
        }
    }
}
