using UnityEngine;
using UnityEngine.Events;

public class ItemReceiver : MonoBehaviour
{
    [SerializeField] private Collider TriggerZone;
    [SerializeField] GameObject TargetItem;
    public UnityEvent OnItemRecevied;
    private void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject == TargetItem)
        {
            TriggerZone.enabled = false;
            if (TargetItem.TryGetComponent(out Rigidbody Rb))
            {
                Rb.isKinematic = true;
                Rb.useGravity = false;
            }
            if (TargetItem.TryGetComponent(out Collider Collider))
            {
                Collider.enabled = false;
            }
            if (TargetItem.TryGetComponent(out PickupItem Component))
            {
                Destroy(Component);
            }
            OnItemRecevied?.Invoke();
        }
    }
}
