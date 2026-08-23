using UnityEngine;

public class DraggingGrip : MonoBehaviour
{
    [SerializeField] private Transform StandPosition;
    [SerializeField] private Transform RightHand;
    [SerializeField] private Transform LeftHand;
    public event System.Action<bool> OnBlocked;
    public void GetStandPosition(out Vector3 Position, out Quaternion Rotation)
    {
        Position = StandPosition.position;
        Rotation = StandPosition.rotation;
    }
    public void GetIK(out Transform RightIK, out Transform LeftIK)
    {
        RightIK = RightHand;
        LeftIK = LeftHand;
    }
    public Vector3 GetForward()
    {
        return transform.forward;
    }
    void OnCollisionEnter(Collision Collision)
    {
        OnBlocked?.Invoke(true);
    }
    void OnCollisionExit(Collision collision)
    {
        OnBlocked?.Invoke(false);
    }
}
