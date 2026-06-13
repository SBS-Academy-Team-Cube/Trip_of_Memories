using UnityEngine;

public class Rope : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "item";
    [SerializeField] private Transform Min, Max;
    public string GetInteractionPrompt()
    {
        return $"E - {itemName} pickup";
    }
    public bool Interact(GameObject Interactor)
    {
        if (Interactor.TryGetComponent(out PlayerRopeHandler Handler))
        {
            if (Handler.TryGrapRope(this))
            {
                return true;
            }
        }
        return false;
    }
    public void GetMinMaxY(out float Min, out float Max)
    {
        Min = this.Min.position.y;
        Max = this.Max.position.y;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
