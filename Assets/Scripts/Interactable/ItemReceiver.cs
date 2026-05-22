using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
    [SerializeField] private Collider TriggerZone;
    [SerializeField] GameObject TargetItem;
    public event System.Action OnItemReceived;
    private bool IsTriggered = false;
    private void Awake()
    {
        if (TriggerZone == null)
        {
            TriggerZone = GetComponent<Collider>();
        }
    }
    private void OnTriggerEnter(Collider Other)
    {
        if (IsTriggered)
        {
            return;
        }

        if (Other.gameObject == TargetItem)
        {
            IsTriggered = true;
            TriggerZone.enabled = false;
            if (TargetItem.TryGetComponent(out PickupItem Component))
            {
                Destroy(Component);
            }
            OnItemReceived?.Invoke();
        }
    }
}
