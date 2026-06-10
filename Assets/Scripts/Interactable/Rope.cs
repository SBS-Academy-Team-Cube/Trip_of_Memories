using System;
using Unity.VisualScripting;
using UnityEngine;

public class Rope : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "item";
    public string GetInteractionPrompt()
    {
        return $"E - {itemName} pickup";
    }
    public bool Interact(GameObject Interactor)
    {
        if (Interactor.TryGetComponent(out PlayerRopeHandler Handler))
        {
            if (Handler.TryGrapRope(gameObject))
            {
                return true;
            }
        }
        return false;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
