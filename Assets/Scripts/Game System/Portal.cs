using UnityEngine;
public class Portal : MonoBehaviour
{
    public event System.Action OnTriggered;
    private void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            OnTriggered?.Invoke();
        }
    }
}
