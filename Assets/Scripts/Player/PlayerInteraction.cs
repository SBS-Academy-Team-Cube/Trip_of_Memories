using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(SphereCollider))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float InteractRadius = 3.0f;

    private List<IInteractable> nearbyInteract = new List<IInteractable>();
    
    private SphereCollider interCollider;
    private IInteractable curTarget;
    


    public void PerformInteraction() // 이벤트호출될때 실행될 함수
    {
        if(curTarget != null)
        {
            curTarget.Interact(gameObject);
            UpdateCurTarget();
        }
    }

    private void Awake()
    {
        //collider setting
        interCollider = GetComponent<SphereCollider>();
        interCollider.radius = InteractRadius;
        interCollider.isTrigger = true;

        if(TryGetComponent<PlayerInputController>(out PlayerInputController InputController))
        {
            InputController.OnInteractPressed.AddListener(PerformInteraction);//event binding
            Debug.Log("OnInteract event binding success");
        } 
    }

    private void OnDestroy()// event unbinding
    {
        PlayerInputController Input = GetComponent<PlayerInputController>();
        if (Input != null)
        {
            Input.OnInteractPressed.RemoveListener(PerformInteraction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IInteractable>(out IInteractable interactable))
            return;

        if(!nearbyInteract.Contains(interactable))
        {
            nearbyInteract.Add(interactable);
            UpdateCurTarget();
            Debug.Log("Update curTarget");
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
