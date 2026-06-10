using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField] private Transform HoldTransform;
    [SerializeField] private PlayerAnimation Animation;
    [SerializeField] private PlayerState State;
    private GameObject HoldingObject;
    public bool bIsHoldingItem { get; private set; } = false;
    public bool TryHold(GameObject Target)
    {
        if (!State.IsGrounded || State.InterAction != PlayerState.EInterAction.None || HoldingObject != null || bIsHoldingItem || State.Stance != PlayerState.EStance.Standing ||
        State.Ability != PlayerState.EAbility.None)
        {
            return false;
        }
        HoldingObject = Target;
        State.SetInterAction(PlayerState.EInterAction.ItemHolding);
        State.IsInteracting = true;
        Animation.SetIsHolding(true);
        return true;
    }
    public void TryDrop()
    {
        if (!State.IsGrounded || !bIsHoldingItem || HoldingObject == null || State.InterAction != PlayerState.EInterAction.ItemHolding)
        {
            return;
        }
        State.IsInteracting = true;
        Animation.SetIsHolding(false);
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
        State.IsInteracting = false;
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
        if (HoldingObject == null)
        {
            return;
        }
        HoldingObject.transform.SetParent(null, true);
        float YRotation = HoldingObject.transform.eulerAngles.y;
        HoldingObject.transform.rotation = Quaternion.Euler(0f, YRotation, 0f);
        Animation.EnableHoldingLayer(false);
        Animation.ClearHandIK();

        Collider[] Colliders = HoldingObject.GetComponentsInChildren<Collider>();
        foreach (Collider Collider in Colliders)
        {
            Collider.enabled = true;
        }
        if (HoldingObject.TryGetComponent(out PickupItem Pickup))
        {
            Pickup.enabled = true;
        }
        if (HoldingObject.TryGetComponent<Rigidbody>(out var RB))
        {
            RB.useGravity = true;
            RB.isKinematic = false;
        }
        HoldingObject = null;
        bIsHoldingItem = false;

        if (State != null)
        {
            State.SetInterAction(PlayerState.EInterAction.None);
            State.IsInteracting = false;
        }
    }
}
