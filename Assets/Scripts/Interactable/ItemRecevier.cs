using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemRecevier : MonoBehaviour
{
    private Collider Collider;
    [SerializeField] GameObject TargetItem;
    [SerializeField] Vector3 ItemFixPosition;
    [SerializeField] List<Mover> Movers;
    [SerializeField] float Duration = 0.3f;
    private void Awake()
    {
        if(!TryGetComponent(out Collider))
        {
            Debug.Log("Can not find Collider in Item Receiver");
        }
    }
    private void OnTriggerEnter(Collider Other)
    {
        if(Other.gameObject == TargetItem)
        {
            StartCoroutine(FixItem(Other.transform));
            foreach (var Mover in Movers)
            {
                Mover.Open();
            }
        }
    }
    private IEnumerator FixItem(Transform Item)
    {
        float time = 0f;
        Vector3 StartPosition = Item.position;
        Vector3 TargetPosition = transform.TransformPoint(ItemFixPosition);

        while (time < Duration)
        {
            float t = time / Duration;

            Item.position = Vector3.Lerp(StartPosition, TargetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        Item.position = TargetPosition;
        Item.SetParent(transform);

    }
}
