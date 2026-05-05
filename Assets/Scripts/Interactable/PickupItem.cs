using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "item";
    [SerializeField] private Transform HoldGrip;
    [SerializeField] private Transform LeftHandIKTarget;
    [SerializeField] private Transform RightHandIKTarget;

    public Transform Grip => HoldGrip ? HoldGrip : transform;
    public Transform LeftHandTarget => LeftHandIKTarget;
    public Transform RightHandTarget => RightHandIKTarget;

    public string GetInteractionPrompt()
    {
        return $"E - {itemName} pickup";
    }
    public bool Interact(GameObject Interactor)
    {
        if (Interactor.TryGetComponent(out PlayerItemHandler Handler))
        {
            Handler.TryHold(gameObject);
            enabled = false;
            return true;
        }
        return false;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
