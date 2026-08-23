using System.Collections.Generic;
using UnityEngine;

public class DraggingKey : MonoBehaviour, IInteractable
{
    [SerializeField] private List<DraggingGrip> Grips;
    private int CurrentDirectionGripIndex = -1;
    public bool CanPush { get; private set; } = true;
    public string GetInteractionPrompt()
    {
        return string.Empty;
    }
    public bool Interact(GameObject Interactor)
    {
        if (Interactor.TryGetComponent(out PlayerDragHandler Handler))
        {
            return Handler.TryGrab(gameObject, GetClosest(Interactor));
        }
        return false;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    private DraggingGrip GetClosest(GameObject Object)
    {
        DraggingGrip Result = null;

        if (CurrentDirectionGripIndex >= 0)
        {
            Grips[CurrentDirectionGripIndex].OnBlocked -= HandleBlocked;
            CurrentDirectionGripIndex = -1;
        }

        float Min = float.MaxValue;
        for (int i = 0; i < Grips.Count; i++)
        {
            DraggingGrip Grip = Grips[i];
            float Distance = Vector3.Distance(Grip.transform.position, Object.transform.position);
            if (Distance < Min)
            {
                Min = Distance;
                Result = Grip;
                CurrentDirectionGripIndex = i + 2 % Grips.Count;
            }
        }
        if (CurrentDirectionGripIndex >= 0)
        {
            Grips[CurrentDirectionGripIndex].OnBlocked += HandleBlocked;
        }
        return Result;
    }
    private void HandleBlocked(bool bBlocked)
    {
        CanPush = !bBlocked;
    }
}
