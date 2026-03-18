using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(SphereCollider))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float interactRadius = 3.0f;

    private List<IInteractable> nearbyInteract = new List<IInteractable>();
    
    private SphereCollider interCollider;

    private CharacterController CharacterController;
    private IInteractable curTarget;

    private void Awake()
    {
        nearbyInteract.Clear();
        interCollider = GetComponent<SphereCollider>();
        interCollider.radius = interactRadius;
        interCollider.isTrigger = true;

        CharacterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(curTarget != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            curTarget.Interact(gameObject);
            UpdateCurTarget();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IInteractable>(out IInteractable interactable))
            return;

        if(!nearbyInteract.Contains(interactable))
        {
            nearbyInteract.Add(interactable);
            UpdateCurTarget();
            Debug.Log($"curTarger : {curTarget.GetInteractionPrompt()}");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<IInteractable>(out IInteractable interactable))
            return;

        nearbyInteract.Remove(interactable);
        UpdateCurTarget();
    }
    private void UpdateCurTarget()
    {
        if(nearbyInteract.Count == 0)
        {
            curTarget = null;
            Debug.Log("curTarget = null");
            return;
        }

        IInteractable closest = null;
        float minDistance = float.MaxValue;
        foreach(var item in nearbyInteract)
        {
            float dist = Vector3.Distance(transform.position,(item as MonoBehaviour).transform.position);
            if(dist < minDistance)
            {
                closest = item;
                minDistance = dist;
            }
        }
        curTarget = closest;
    }
}
