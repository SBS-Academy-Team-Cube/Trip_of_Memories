using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "item";


    public string GetInteractionPrompt()
    {
        return $"E - {itemName} pickup";
    }

    public void Interact(GameObject Interactor)
    {
        if(Interactor.TryGetComponent(out PlayerItemHandler Handler))
        {
            Debug.Log("Interacter has ItemHandler Component!!");
            Handler.HoldItem(gameObject);
            enabled = false;
        }
    }
}
