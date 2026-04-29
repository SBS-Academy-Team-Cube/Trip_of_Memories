using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField] private Transform HoldTransform;

    private GameObject HoldingObject;
    public bool bIsHoldingItem { get; private set; } = false;

    public void HoldItem(GameObject Target)
    {
        if (Target == null)
        {
            return;
        }
        HoldingObject = Target;
        Target.transform.SetParent(HoldTransform, true);
        Target.transform.position = HoldTransform.position;
        // Target.transform.localPosition = Vector3.zero;
        // Target.transform.localRotation = Quaternion.identity;
        if (Target.TryGetComponent(out HoverItem HoverComponent))
        {
            Destroy(HoverComponent);
        }

        if (Target.TryGetComponent<Rigidbody>(out var RB))
        {
            RB.isKinematic = true;
            RB.useGravity = false;
        }
        if (Target.TryGetComponent<Collider>(out var Collider))
        {
            Collider.enabled = false;
        }
        bIsHoldingItem = true;
    }

    public void DropItem()
    {
        if (!bIsHoldingItem || HoldingObject == null)
        {
            return;
        }
        HoldingObject.transform.SetParent(null, true);
        if (HoldingObject.TryGetComponent<Rigidbody>(out var RB))
        {
            RB.isKinematic = false;
            RB.useGravity = true;
            RB.AddForce(transform.forward * 50f, ForceMode.Impulse);
        }
        if (HoldingObject.TryGetComponent<Collider>(out var Collider))
        {
            Collider.enabled = true;
        }
        HoldingObject = null;
        bIsHoldingItem = false;
    }
}
