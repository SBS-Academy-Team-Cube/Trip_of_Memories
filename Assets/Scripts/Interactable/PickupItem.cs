using UnityEngine;

public class PickupItem : MonoBehaviour,IInteractable
{
    [SerializeField] private string itemName = "item";


    public string GetInteractionPrompt()
    {
        return $"E - {itemName} pickup";
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} - {itemName} pickup!");
        Destroy(gameObject);
    }


}
