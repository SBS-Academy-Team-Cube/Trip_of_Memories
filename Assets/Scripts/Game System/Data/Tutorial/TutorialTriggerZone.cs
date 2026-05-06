using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggerZone : MonoBehaviour
{
    [SerializeField] private int Index;
    private bool Triggered = false;
    public UnityEvent<int> OnTriggered;
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
        OnTriggered?.Invoke(Index);
    }
}