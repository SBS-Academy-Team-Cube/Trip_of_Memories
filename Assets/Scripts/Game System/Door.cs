using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Trigger[] Triggers;
    [SerializeField] private Mover DoorMover;
    [SerializeField] private AudioSource Audio;
    private int RequiredCondition = 0;
    private int CurrentCondition = 0;

    private void Awake()
    {
        RequiredCondition = Triggers.Length;
    }
    private void OnEnable()
    {
        foreach (var Trigger in Triggers)
        {
            Trigger.OnTriggered += HandleCondition;
        }
    }
    private void OnDisable()
    {
        foreach (var Trigger in Triggers)
        {
            Trigger.OnTriggered -= HandleCondition;
        }
    }
    private void HandleCondition()
    {
        CurrentCondition++;
        if (CurrentCondition == RequiredCondition)
        {
            DoorMover.Move();
            if (Audio && Audio.clip && AudioManager.Instance)
            {
                Audio.volume *= AudioManager.Instance.SFX_VOLUME;
                Audio.Play();
            }
        }
    }
}