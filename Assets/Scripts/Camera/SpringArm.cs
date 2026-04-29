using UnityEngine;
using Unity.Cinemachine;

public class SpringArm : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Transform CameraTransform;
    [SerializeField] private LayerMask ObstacleLayer;
    [SerializeField] private float CameraRadius = 0.3f;
    [SerializeField] private CinemachineOrbitalFollow OrbitalFollowComponent;
    [SerializeField] private float SmoothSpeed = 10f;
    private float CurrentRadius;
    public float OriginRadius = 2.0f;
    public float MinRadius = 0.75f;
    public void SetTarget(Transform TargetTransform)
    {
        Target = TargetTransform;
    }
    private void LateUpdate()
    {
        if (!Target)
        {
            return;
        }
        Vector3 Direction = CameraTransform.position - Target.position;
        float Distance = Direction.magnitude;
        Direction.Normalize();

        RaycastHit Hit;
        float targetRadius;

        if (Physics.SphereCast(Target.position, CameraRadius, Direction, out Hit, OriginRadius + 0.25f, ObstacleLayer))
        {
            targetRadius = Mathf.Clamp(Hit.distance, MinRadius, Distance);
        }
        else
        {
            targetRadius = OriginRadius;
        }
        CurrentRadius = Mathf.Lerp(CurrentRadius, targetRadius, Time.deltaTime * SmoothSpeed);

        OrbitalFollowComponent.Radius = CurrentRadius;
    }
}
