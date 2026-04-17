using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class ItemReceiver : MonoBehaviour
{
    [SerializeField] private Collider TriggerZone;
    [SerializeField] GameObject TargetItem;
    [SerializeField] Vector3 ItemFixPosition;
    [SerializeField] float Duration = 0.3f;
    public UnityEvent OnItemRecevied;
    private void OnTriggerEnter(Collider Other)
    {
        if(Other.gameObject == TargetItem)
        {
            TriggerZone.enabled = false;
            if(TargetItem.TryGetComponent(out Rigidbody Rb))
            {
                Rb.isKinematic = true;
                Rb.useGravity = false;
            }
            if(TargetItem.TryGetComponent(out Collider Collider))
            {
                Collider.enabled = false;
            }
            OnItemRecevied?.Invoke();
            StartCoroutine(FixItem(Other.gameObject.transform));
        }
    }
    private IEnumerator FixItem(Transform Item)
    {
        Debug.Log("Fix Routine Started");
        float time = 0f;
        Vector3 StartPosition = Item.position;
        Vector3 TargetPosition = transform.TransformPoint(ItemFixPosition);
        while (time < Duration)
        {
            float t = time / Duration;
            time += Time.deltaTime;
            Item.position = Vector3.Lerp(StartPosition, TargetPosition, t);
            yield return null;
        }
        Item.position = TargetPosition;
        Item.SetParent(transform);
    }
}
