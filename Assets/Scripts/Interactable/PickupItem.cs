using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "item";
    public string GetInteractionPrompt()
    {
        return $"E - {itemName} pickup";
    }
    public bool Interact(GameObject Interactor)
    {
        if(Interactor.TryGetComponent(out PlayerItemHandler Handler))
        {
            Handler.HoldItem(gameObject);
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
