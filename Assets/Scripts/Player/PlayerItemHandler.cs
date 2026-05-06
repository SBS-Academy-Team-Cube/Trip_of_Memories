using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField] private Transform HoldTransform;
    [SerializeField] private PlayerAnimation Animation;
    private GameObject HoldingObject;
    public bool bIsHoldingItem { get; private set; } = false;

    public void TryHold(GameObject Target)
    {
        HoldingObject = Target;
        Animation.SetPickup();
    }
    public void TryDrop()
    {
        if (!bIsHoldingItem || HoldingObject == null)
        {
            return;
        }
        Animation.SetPickDown();
    }
    public void HoldItem()
    {
        if (HoldingObject == null)
        {
            return;
        }
        if (HoldingObject.TryGetComponent(out PickupItem Pickup))
        {
            AlignItemGripToHoldTransform(HoldingObject.transform, Pickup.Grip);
            Animation.SetHandIKTargets(Pickup.LeftHandTarget, Pickup.RightHandTarget);
            Animation.SetHandIKWeight(Pickup.LeftHandTarget ? 1.0f : 0.0f, 0.0f);
        }
        else
        {
            HoldingObject.transform.position = HoldTransform.position;
            HoldingObject.transform.rotation = HoldTransform.rotation;
            Animation.ClearHandIK();
        }

        HoldingObject.transform.SetParent(HoldTransform, true);
        Animation.EnableHoldingLayer(true);

        // Target.transform.localPosition = Vector3.zero;
        // Target.transform.localRotation = Quaternion.identity;
        if (HoldingObject.TryGetComponent(out HoverItem HoverComponent))
        {
            HoverComponent.enabled = false;
        }
        if (HoldingObject.TryGetComponent<Rigidbody>(out var RB))
        {
            RB.isKinematic = true;
            RB.useGravity = false;
        }
        Collider[] Colliders = HoldingObject.GetComponentsInChildren<Collider>();
        foreach (Collider Collider in Colliders)
        {
            Collider.enabled = false;
        }
        bIsHoldingItem = true;
    }
    private void AlignItemGripToHoldTransform(Transform itemRoot, Transform grip)
    {
        Quaternion RotationDelta = HoldTransform.rotation * Quaternion.Inverse(grip.rotation);
        itemRoot.rotation = RotationDelta * itemRoot.rotation;

        Vector3 positionDelta = HoldTransform.position - grip.position;
        itemRoot.position += positionDelta;
    }

    public void DropItem()
    {
        HoldingObject.transform.SetParent(null, true);
        Animation.EnableHoldingLayer(false);
        Animation.ClearHandIK();

        Collider[] Colliders = HoldingObject.GetComponentsInChildren<Collider>();
        foreach (Collider Collider in Colliders)
        {
            Collider.enabled = true;
        }
        if (HoldingObject.TryGetComponent<Rigidbody>(out var RB))
        {
            RB.isKinematic = false;
            RB.useGravity = true;
        }
        if (HoldingObject.TryGetComponent(out PickupItem Pickup))
        {
            Pickup.enabled = true;
        }
        HoldingObject = null;
        bIsHoldingItem = false;
    }
}
