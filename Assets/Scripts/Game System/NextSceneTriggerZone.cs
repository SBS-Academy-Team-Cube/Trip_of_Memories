using UnityEngine;
using UnityEngine.Events;

public class NextSceneTriggerZone : MonoBehaviour
{
    public UnityEvent OnLoadNextScene;
    
    private bool IsTriggered = false;
    private void OnTriggerEnter(Collider Other)
    {
        if(Other.CompareTag("Player") && !IsTriggered)
        {
            IsTriggered = true;
            OnLoadNextScene?.Invoke();
        }
    }
}
