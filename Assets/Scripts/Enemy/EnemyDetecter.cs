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
    public bool IsDetected = false;
    void OnTriggerStay(Collider Other)
    {
        if (IsDetected)
        {
            return;
        }
        if (Other.CompareTag(PlayerTag))
        {
            Debug.Log("Detected!");
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
            Debug.Log("Player Out Detected!");
            IsDetected = false;
            OnPlayerDetected?.Invoke(false, null);
            DetectingTrigger.radius = DetectingRadius;
        }
    }
}
