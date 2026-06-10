using System;
using UnityEngine;
using UnityEngine.XR;

[Serializable]
public struct FLeverPosition
{
    public Transform StandPosition;
    public Transform LeftHandIKPosition;
    public Transform RightHandIKPosition;
    public Vector3 GetPosition()
    {
        return StandPosition.position;
    }
}
public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "item";
    [SerializeField] private float RotateSpeed;
    [SerializeField] private Rotator[] TargetRotators;
    [SerializeField] private Transform LeverHandleTransform;
    [SerializeField] private FLeverPosition Clockwise;
    [SerializeField] private FLeverPosition CounterClockwise;
    [SerializeField] private Trigger Trigger;

    public bool IsRotating = false;
    public bool bClockwise { get; private set; }
    public string GetInteractionPrompt()
    {
        return itemName;
    }
    public void GetIKPosition(out Transform Left, out Transform Right)
    {
        Left = bClockwise ? CounterClockwise.LeftHandIKPosition : Clockwise.LeftHandIKPosition;
        Right = bClockwise ? CounterClockwise.RightHandIKPosition : Clockwise.RightHandIKPosition;
    }
    public Vector3 GetPosition()
    {
        return bClockwise ? CounterClockwise.GetPosition() : Clockwise.GetPosition();
    }
    public bool Interact(GameObject Interactor)
    {
        if (Interactor.TryGetComponent(out PlayerLeverHandler Handler))
        {
            bClockwise = GetClockwise(transform, LeverHandleTransform, Interactor.transform);
            Handler.HandleLever(this);
            if (Trigger != null)
            {
                Trigger.OnTrigger();
            }
        }
        return true;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void SetIsRotating(bool bRotating)
    {
        IsRotating = bRotating;
        if (TargetRotators.Length > 0)
        {
            foreach (Rotator Rotator in TargetRotators)
            {
                if (bRotating)
                {
                    Rotator.StartRotate(bClockwise ? -1f : 1f);
                }
                else
                {
                    Rotator.EndRotate();
                }
            }
        }
    }
    private void Update()
    {
        if (IsRotating)
        {
            Rotate();
        }
    }
    private bool GetClockwise(Transform Pivot, Transform Handle, Transform Interactor)
    {

        Vector2 A = new Vector2(Handle.position.x - Pivot.position.x, Handle.position.z - Pivot.position.z);
        Vector2 B = new Vector2(Interactor.position.x - Pivot.position.x, Interactor.position.z - Pivot.position.z);
        float Cross = A.x * B.y - A.y * B.x;
        return Cross < 0f;
    }
    public void Rotate()
    {
        transform.Rotate(Vector3.up, RotateSpeed * (bClockwise ? -1f : 1f) * Time.deltaTime);
    }
    public void Release()
    {
        IsRotating = false;
        if (Trigger != null)
        {
            Trigger.OnTrigger();
        }
    }
}
