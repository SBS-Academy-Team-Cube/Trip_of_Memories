using UnityEngine;
using UnityEngine.XR;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "item";
    [SerializeField] private float RotateSpeed;
    [SerializeField] private Rotator TargetRotator;
    [SerializeField] Transform LeverHandleTransform;
    public bool IsRotating = false;
    private bool bClockwise;
    public string GetInteractionPrompt()
    {
        return itemName;
    }
    public bool Interact(GameObject Interactor)
    {
        if (Interactor.TryGetComponent(out PlayerLeverHandler Handler))
        {
            bClockwise = GetClockwise(transform, LeverHandleTransform, Interactor.transform);
            Debug.Log(bClockwise);
            Handler.HandleLever(gameObject, !bClockwise);
            return true;
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
        if (TargetRotator)
        {
            if (bRotating)
            {
                TargetRotator.StartRotate(bClockwise ? -1f : 1f);
            }
            else
            {
                TargetRotator.EndRotate();
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
}
