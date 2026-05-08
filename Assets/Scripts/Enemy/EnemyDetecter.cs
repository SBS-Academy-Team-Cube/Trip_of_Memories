using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetecter : MonoBehaviour
{
    public event Action<bool, Transform> OnPlayerDetected;
    [SerializeField] private string PlayerTag = "Player";
    [SerializeField] private float DetectingRadius = 5.0f;
    [SerializeField] private float DetectedRadius = 6.0f;
    [SerializeField] private SphereCollider DetectingTrigger;
    private bool IsDetected = false;
    void OnTriggerEnter(Collider Other)
    {
        if (IsDetected)
        {
            return;
        }
        if (Other.CompareTag(PlayerTag))
        {
            IsDetected = true;
            OnPlayerDetected?.Invoke(true, Other.transform);
            DetectingTrigger.radius = DetectedRadius;
        }
    }
    void OnTriggerExit(Collider Other)
    {
        if (!IsDetected)
        {
            return;
        }
        if (Other.CompareTag(PlayerTag))
        {
            IsDetected = true;
            OnPlayerDetected?.Invoke(false, null);
            DetectingTrigger.radius = DetectingRadius;
        }
    }
}
