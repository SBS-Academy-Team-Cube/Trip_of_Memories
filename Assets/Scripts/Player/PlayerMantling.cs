using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

public class PlayerMantling : MonoBehaviour
{
    [Header("Mantle Settings")]
    public float LedgeCheckForwardDistance = 1.0f;
    [SerializeField] private PlayerAnimation Animation;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform LedgePoint;
    [SerializeField] private PlayerMovement Move;
    public bool IsMantling { get; private set; }
    private float HandIKWeight;
    Vector3 LeftHandTarget;
    Vector3 RightHandTarget;
    Vector3 WallNormal;

    void OnAnimatorIK(int layerIndex)
    {
        if (!IsMantling)
        {
            return;
        }
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKWeight);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandIKWeight);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget);
        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget);

        Quaternion rot = Quaternion.LookRotation(-WallNormal);

        animator.SetIKRotation(AvatarIKGoal.LeftHand, rot);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rot);
    }
    void Update()
    {
        HandIKWeight = animator.GetFloat("HandIKWeight");
    }
    public void DoMantling(Vector3 TargetPosition)
    {
        IsMantling = true;
        Controller.enabled = false;

        transform.rotation = Quaternion.LookRotation(-WallNormal);
        transform.position += TargetPosition - LedgePoint.position;

        CalculateHandTargets(TargetPosition);
        Animation.SetMantling();
    }
    public void OnEndMantling()
    {
        IsMantling = false;
        Controller.enabled = true;
        Animation.DisableRootMotion();
    }
    public void TryMantling()
    {
        if (IsMantling || !LedgeCheck(out Vector3 TargetPosition))
        {
            return;
        }
        DoMantling(TargetPosition);
    }
    void CalculateHandTargets(Vector3 TargetPosition)
    {
        Vector3 right = Vector3.Cross(Vector3.up, WallNormal).normalized;
        float handOffset = -0.2f;
        LeftHandTarget = TargetPosition - right * handOffset;
        RightHandTarget = TargetPosition + right * handOffset;

        DebugExtension.DrawSphere(LeftHandTarget, 0.2f, Color.blue, 1f);
        DebugExtension.DrawSphere(RightHandTarget, 0.2f, Color.red, 1f);
    }
    private bool LedgeCheck(out Vector3 OutLedgePosition)
    {
        OutLedgePosition = Vector3.zero;

        Vector3 LedgetCheckDirection = new Vector3(Move.CameraTransform.forward.x, 0f, Move.CameraTransform.forward.z);
        LedgetCheckDirection.Normalize();
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit WallHit, LedgeCheckForwardDistance))
        {
            // Wall Checking Debug Line
            Debug.DrawLine(transform.position, transform.position + transform.forward * LedgeCheckForwardDistance, Color.green);
            if (!WallHit.collider.CompareTag("ClimbableWall"))
            {
                return false;
            }
            WallNormal = WallHit.normal;

            // Wall Hit Point Debug Sphere
            DebugExtension.DrawSphere(WallHit.point, 0.05f, Color.green, 0.25f);

            Vector3 PlanCheckRayStartPosition = WallHit.point + Vector3.up * (LedgePoint.localPosition.y + 0.1f) - WallHit.normal * 0.075f;

            // Ledge Plane Check Start Position Debug Sphere
            DebugExtension.DrawSphere(PlanCheckRayStartPosition, 0.05f, Color.red, 0.25f);

            if (Physics.Raycast(PlanCheckRayStartPosition, Vector3.down, out RaycastHit LedgeHit, LedgePoint.localPosition.y + 0.1f))
            {
                // Ledge Plane Check Debug Line
                Debug.DrawLine(PlanCheckRayStartPosition, PlanCheckRayStartPosition + Vector3.down * PlanCheckRayStartPosition.y, Color.red);
                if (!LedgeHit.collider.CompareTag("ClimbableWall"))
                {
                    return false;
                }
                OutLedgePosition = LedgeHit.point;
                // Target Mantling Position Debug Sphere
                DebugExtension.DrawSphere(OutLedgePosition, 0.05f, Color.orange, 0.25f);
                return true;
            }
        }
        return false;
    }
}


public static class DebugExtension
{
    public static void DrawSphere(Vector3 position, float radius, Color color, float duration = 0f)
    {
        int segments = 16;

        float angleStep = 360f / segments;

        // XY 평면
        for (int i = 0; i < segments; i++)
        {
            float angle1 = Mathf.Deg2Rad * (i * angleStep);
            float angle2 = Mathf.Deg2Rad * ((i + 1) * angleStep);

            Vector3 p1 = position + new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0) * radius;
            Vector3 p2 = position + new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0) * radius;

            Debug.DrawLine(p1, p2, color, duration);
        }

        // XZ 평면
        for (int i = 0; i < segments; i++)
        {
            float angle1 = Mathf.Deg2Rad * (i * angleStep);
            float angle2 = Mathf.Deg2Rad * ((i + 1) * angleStep);

            Vector3 p1 = position + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
            Vector3 p2 = position + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;

            Debug.DrawLine(p1, p2, color, duration);
        }

        // YZ 평면
        for (int i = 0; i < segments; i++)
        {
            float angle1 = Mathf.Deg2Rad * (i * angleStep);
            float angle2 = Mathf.Deg2Rad * ((i + 1) * angleStep);

            Vector3 p1 = position + new Vector3(0, Mathf.Cos(angle1), Mathf.Sin(angle1)) * radius;
            Vector3 p2 = position + new Vector3(0, Mathf.Cos(angle2), Mathf.Sin(angle2)) * radius;

            Debug.DrawLine(p1, p2, color, duration);
        }
    }
}