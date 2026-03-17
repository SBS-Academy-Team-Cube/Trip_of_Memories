using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float interactionDistance = 3.0f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform CameraTransform;

    private IInteractable curInteractable;

    private void Update()
    {
        HandleInteractionRay();
    }

    private void HandleInteractionRay()
    {
        Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);
        if(Physics.Raycast(ray,out RaycastHit hit,interactionDistance, interactableLayer))
        {
            IInteractable inter = hit.collider.GetComponent<IInteractable>();
            if(inter != null)
            {
                if(inter != curInteractable)
                {
                    curInteractable = inter;
                    Debug.Log($"PlayerInteractable.cs - HandleInteractinRay() - " +
                        $"Look ->{inter.GetInteractionPrompt()}");
                }
                if(Keyboard.current.eKey.wasPressedThisFrame)
                {
                    inter.Interact(gameObject);
                }
            }
            else
            {
                curInteractable = null;
            }
        }
        else
        {
            curInteractable = null;
        }
    }


}
