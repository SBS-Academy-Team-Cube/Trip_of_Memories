using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable CurTarget = null;
    private List<IInteractable> InteractableList = new List<IInteractable>();
    public void PerformInteraction()
    {
        if (CurTarget != null)
        {
            var Target = CurTarget;
            CurTarget = null;
            if (Target.Interact(gameObject))
            {
                InteractableList.Remove(Target);
            }
            UpdateCurTarget();
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (!other.TryGetComponent(out IInteractable interactable))
        {
            return;
        }
        if (InteractableList.Contains(interactable))
        {
            return;
        }
        InteractableList.Add(interactable);
        UpdateCurTarget();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable interactable))
        {
            return;
        }
        if (!InteractableList.Contains(interactable))
        {
            return;
        }
        InteractableList.Remove(interactable);
        UpdateCurTarget();
    }
    private void UpdateCurTarget()
    {
        if (InteractableList.Count == 0)
        {
            CurTarget = null;
            return;
        }
        IInteractable closest = null;
        float minDistance = float.MaxValue;
        Vector3 PlayerPos = transform.position;

        for (int i = InteractableList.Count - 1; i >= 0; i--)
        {
            var item = InteractableList[i];
            if (item is MonoBehaviour mono)
            {
                if (mono != null && mono.gameObject.activeInHierarchy)
                {
                    float dist = Vector3.Distance(PlayerPos, mono.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closest = item;
                    }
                }

            }
            else
            {
                InteractableList.RemoveAt(i);
            }
        }
        CurTarget = closest;
    }
}
