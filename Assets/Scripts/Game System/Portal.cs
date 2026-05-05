using UnityEngine;
using UnityEngine.Events;
public class Portal : MonoBehaviour
{
    public Transform test;
    public UnityEvent<Vector3> OnTriggered;
    private void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            OnTriggered?.Invoke(/*Other.transform*/test.position);
        }
    }
}
