using System.Collections.Generic;
using UnityEngine;




public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerItemHandler ItemHandler;
    private IInteractable CurTarget;
    private List<IInteractable> InteractableList;
    public System.Action<string, Transform, bool> OnTargetChanged;
    public void PerformInteraction()
    {
        if (ItemHandler.bIsHoldingItem)
        {
            ItemHandler.DropItem();
            return;
        }
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

    private void Awake()
    {
        InteractableList = new List<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Return if interaction is not possible
        if (!other.TryGetComponent(out IInteractable interactable))
            return;
        // Throw error if already present in the list
        if (InteractableList.Contains(interactable))
            return;
        // Add to list and set curTarget based on distance comparison
        InteractableList.Add(interactable);
        UpdateCurTarget();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable interactable))
        {
            return;
        }
        // Throw error if not present in the list
        if (!InteractableList.Contains(interactable))
            return;
        InteractableList.Remove(interactable);
        UpdateCurTarget();
    }
    private void UpdateCurTarget()
    {
        if (InteractableList.Count == 0)
        {
            CurTarget = null;
            OnTargetChanged?.Invoke(null, null, false);
            return;
        }

        IInteractable closest = null;
        float minDistance = float.MaxValue;
        Vector3 PlayerPos = transform.position;

        for (int i = InteractableList.Count - 1; i >= 0; i--)
        {
            var item = InteractableList[i];

            // Check if it is MonoBehaviour and actually exists
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
                // Removed from list if SetActive(false) was called by interaction
                InteractableList.RemoveAt(i);
            }
        }
        CurTarget = closest;
        if (CurTarget != null)
        {
            OnTargetChanged?.Invoke(CurTarget.GetInteractionPrompt(), CurTarget.GetTransform(), true);
        }
    }
}
