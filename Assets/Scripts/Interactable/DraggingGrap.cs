using UnityEngine;

public class DraggingGrap : MonoBehaviour
{
    [SerializeField] private Transform StandPosition;
    [SerializeField] private Transform RightHand;
    [SerializeField] private Transform LeftHand;
    public event System.Action OnBlocked;
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
        OnBlocked?.Invoke();
    }
}
