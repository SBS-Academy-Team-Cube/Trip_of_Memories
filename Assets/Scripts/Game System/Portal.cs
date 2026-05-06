using UnityEngine;
using UnityEngine.Events;
public class Portal : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    public UnityEvent<Vector3> OnTriggered;
    private void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            // Other.TryGetComponent(out PlayerMovement move);
            OnTriggered?.Invoke(new Vector3(0.5f, 0.5f, 0.0f)/*MainCamera.WorldToViewportPoint(move.CameraPivot.position)*/);
        }
    }
}
