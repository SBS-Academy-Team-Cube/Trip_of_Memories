using System.Collections.Generic;
using UnityEngine;

public class DraggingKey : MonoBehaviour, IInteractable
{
    [SerializeField] private List<DraggingGrap> Graps;
    public string GetInteractionPrompt()
    {
        return string.Empty;
    }
    public bool Interact(GameObject Interactor)
    {
        if (Interactor.TryGetComponent(out PlayerDragHandler Handler))
        {
            return Handler.TryGrap(gameObject, GetClosest(Interactor));
        }
        return false;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    private DraggingGrap GetClosest(GameObject Object)
    {
        DraggingGrap Result = null;
        float Min = float.MaxValue;
        foreach (DraggingGrap Grap in Graps)
        {
            float Distance = Vector3.Distance(Grap.transform.position, Object.transform.position);
            if (Distance < Min)
            {
                Min = Distance;
                Result = Grap;
            }
        }
        return Result;
    }
}
