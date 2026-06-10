using UnityEngine;
using UnityEngine.Events;

public class Rotator : MonoBehaviour
{
    public float RotateSpeed = 30f;
    public UnityEvent OnRotateStart;
    public UnityEvent OnRotateEnd;
    private bool IsRotating = false;
    private float Direction;
    [SerializeField] private float MaxYaw, MinYaw;
    
    public void StartRotate(float ClockwiseDirection)
    {
        IsRotating = true;
        Direction = ClockwiseDirection;
        OnRotateStart?.Invoke();
    }
    public void EndRotate()
    {
        IsRotating = false;
        OnRotateEnd?.Invoke();
    }
    void Update()
    {
        if (IsRotating)
        {
            transform.Rotate(0f, Direction * RotateSpeed * Time.deltaTime, 0f);
        }
    }
}
