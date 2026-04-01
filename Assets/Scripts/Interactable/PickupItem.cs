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
        Debug.Log($"item pickup!");
        gameObject.SetActive(false);
    }
}
