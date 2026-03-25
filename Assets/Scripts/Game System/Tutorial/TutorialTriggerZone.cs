using UnityEngine;

public class TutorialTriggerZone : MonoBehaviour
{
    public static System.Action<int> OnTriggered;
    [SerializeField] private int MyIndex;
    private bool Triggered = false;

    private void OnTriggerEnter(Collider Other) 
    {
        if (Triggered) 
        { 
            return;
        }
        if (!Other.CompareTag("Player")) 
        { 
            return;
        }
        Triggered = true;
        OnTriggered?.Invoke(MyIndex);  
    }
}
