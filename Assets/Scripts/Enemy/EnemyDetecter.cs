using System;
using UnityEngine;

public class EnemyDetecter : MonoBehaviour
{
    public event Action<bool, Transform> OnPlayerDetected;

    [SerializeField] private string PlayerTag = "Player";
    [SerializeField] private float DetectingRadius = 5.0f;
    [SerializeField] private float DetectedRadius = 6.0f;
    [SerializeField] private SphereCollider DetectingTrigger;
    public bool IsDetected = false;
    public Transform DetectedPlayer { get; private set; }

    private void Awake()
    {
        if (DetectingTrigger == null)
        {
            DetectingTrigger = GetComponent<SphereCollider>();
        }
        if (DetectingTrigger != null)
        {
            DetectingTrigger.radius = DetectingRadius;
        }
    }
    void OnTriggerStay(Collider Other)
    {
        if (IsDetected)
        {
            return;
        }
        if (Other.CompareTag(PlayerTag))
        {
            IsDetected = true;
            DetectedPlayer = Other.transform;
            OnPlayerDetected?.Invoke(true, Other.transform);
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
            IsDetected = false;
            DetectedPlayer = null;
            OnPlayerDetected?.Invoke(false, null);
            SetFocused(false);
        }
    }
    public void SetFocused(bool IsFocused)
    {
        if (DetectingTrigger == null)
        {
            return;
        }
        DetectingTrigger.radius = IsFocused ? DetectedRadius : DetectingRadius;
    }
}
