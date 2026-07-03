using UnityEngine;
using UnityEngine.XR;
public class PlayerMantling : MonoBehaviour
{
    [Header("Mantle Settings")]
    [SerializeField] private float FrontWallCheckDistance = 1.5f;
    [SerializeField] private float LedgeAllowDistance = 1.0f;
    [SerializeField] private float handOffset = .125f;
    [SerializeField] private float handUpOffset = .05f;
    [SerializeField] private LayerMask ClimbableWallLayer = ~0;
    [SerializeField] private PlayerAnimation Animation;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform LedgePoint;
    [SerializeField] private PlayerMovement Move;


    public bool IsMantling { get; private set; }

    private Vector3 LedgeTargetPoint = Vector3.zero;

    Vector3 LeftHandTarget;
    Vector3 RightHandTarget;
    Vector3 WallNormal;
    private Transform LeftHandIKTargetTransform;
    private Transform RightHandIKTargetTransform;

    private void Awake()
    {
        EnsureHandIKTargetTransforms();
    }

    public void DoMantling(Vector3 TargetPosition)
    {
        IsMantling = true;
        Controller.enabled = false;

        WallNormal = TargetPosition - transform.position;
        WallNormal.y = 0.0f;
        WallNormal.Normalize();

        transform.SetPositionAndRotation(transform.position + (TargetPosition - LedgePoint.position), Quaternion.LookRotation(WallNormal));

        CalculateHandTargets(TargetPosition);
        ApplyHandIKTargetTransforms();
        Animation.SetMantling();
        Animation.SetHandIKTargets(LeftHandIKTargetTransform, RightHandIKTargetTransform);
        Animation.SetHandIKWeight(1.0f, 1.0f);
    }
    public void ReleaseHand()
    {
        Animation.ClearHandIK();
    }
    public void OnEndMantling()
    {
        IsMantling = false;
        Controller.enabled = true;
        Animation.ClearHandIK();
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
        LeftHandTarget = TargetPosition - right * handOffset + Vector3.up * handUpOffset;
        RightHandTarget = TargetPosition + right * handOffset + Vector3.up * handUpOffset;
    }

    private void EnsureHandIKTargetTransforms()
    {
        if (!LeftHandIKTargetTransform)
        {
            LeftHandIKTargetTransform = new GameObject($"{name}_Mantle_LeftHandIKTarget").transform;
        }
        if (!RightHandIKTargetTransform)
        {
            RightHandIKTargetTransform = new GameObject($"{name}_Mantle_RightHandIKTarget").transform;
        }
    }

    private void ApplyHandIKTargetTransforms()
    {
        EnsureHandIKTargetTransforms();

        Quaternion handRotation = Quaternion.LookRotation(WallNormal);
        LeftHandIKTargetTransform.SetPositionAndRotation(LeftHandTarget, handRotation);
        RightHandIKTargetTransform.SetPositionAndRotation(RightHandTarget, handRotation);
    }

    private void OnDestroy()
    {
        if (LeftHandIKTargetTransform)
        {
            Destroy(LeftHandIKTargetTransform.gameObject);
        }
        if (RightHandIKTargetTransform)
        {
            Destroy(RightHandIKTargetTransform.gameObject);
        }
    }

    private bool LedgeCheck(out Vector3 OutLedgePosition)
    {
        OutLedgePosition = Vector3.zero;
        Debug.DrawLine(transform.position, transform.position + transform.forward * FrontWallCheckDistance, Color.green, 10.0f);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit WallHit, FrontWallCheckDistance, ClimbableWallLayer))
        {
            // Wall Checking Debug Line
            Debug.DrawLine(transform.position, transform.position + transform.forward * FrontWallCheckDistance, Color.red, 10.0f);
            WallNormal = WallHit.normal;

            // Wall Hit Point Debug Sphere
            DebugExtension.DrawSphere(WallHit.point, 0.05f, Color.green, 0.25f);

            if (WallHit.collider.TryGetComponent(out Ledge ledge))
            {
                if (ledge.TryGetLedgePoint(LedgePoint.position, out OutLedgePosition, out float distance))
                {
                    return distance <= LedgeAllowDistance;
                }
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
