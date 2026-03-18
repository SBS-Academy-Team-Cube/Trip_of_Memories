using System.Collections.Generic;
using UnityEngine;




public class PlayerInteraction : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float InteractRadius = 3.0f;
    
    private SphereCollider PlayerInteractCollider;

    private IInteractable CurTarget; 
    private List<IInteractable> InteractableList; 

    public void PerformInteraction()
    {
        if(CurTarget != null)
        {
            var Target = CurTarget;
            CurTarget = null;
            
            Target.Interact(gameObject);

            UpdateCurTarget();
        }
    }

    private void Awake()
    {
        if(!TryGetComponent<SphereCollider>(out PlayerInteractCollider))
        {
            PlayerInteractCollider = gameObject.AddComponent<SphereCollider>();
        }
        //collider setting
        PlayerInteractCollider.radius = InteractRadius;
        PlayerInteractCollider.isTrigger = true;

        InteractableList = new List<IInteractable>();
    }


    private void OnTriggerEnter(Collider other)
    {
        // 상호작용 불가능하면 리턴
        if (!other.TryGetComponent<IInteractable>(out IInteractable interactable))
            return;
        // 이미 리스트에있다면 오류상황
        if (InteractableList.Contains(interactable))
            return;

        //리스트에 추가하고 거리비교로 curTarget 설정
        InteractableList.Add(interactable);
        UpdateCurTarget();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<IInteractable>(out IInteractable interactable))
            return;
        //리스트에 없다면 오류
        if (!InteractableList.Contains(interactable))
            return;

        InteractableList.Remove(interactable);
        UpdateCurTarget();
    }
    private void UpdateCurTarget()
    {
        if(InteractableList.Count == 0)
        {
            CurTarget = null;
            Debug.Log("curTarget = null");
            return;
        }

        IInteractable closest = null;
        float minDistance = float.MaxValue;
        Vector3 PlayerPos = transform.position;

        for (int i = InteractableList.Count - 1; i >= 0; i--)
        {
            var item = InteractableList[i];

            // MonoBehaviour인지, 실제로 존재하는지 체크
            if (item is MonoBehaviour mono)
            {
                if(mono != null && mono.gameObject.activeInHierarchy)
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
                // 상호작용으로 SetActive(false)되었으면 리스트에서 삭제
                InteractableList.RemoveAt(i);
            }
        }

        CurTarget = closest;
        Debug.Log("Update curTarget");
    }
}
