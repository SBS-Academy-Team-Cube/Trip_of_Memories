using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

public class PlayerMantling : MonoBehaviour
{
    [Header("Mantle Settings")]
    public float MantleHeight = 1.5f;
    public float ForwardDistance = 1.0f;
    public float LedgeCheckDistance = 10.0f;
    [SerializeField] private PlayerAnimation Animation;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController Controller;
    private bool IsMantling;
    private bool bCanMantling = false;
    Vector3 MantlingTargetPosition;

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
        LedgeCheck();
        HandIKWeight = animator.GetFloat("HandIKWeight");
    }
    public void DoMantling()
    {
        Controller.enabled = false;
        Animation.SetMantling();

        CalculateHandTargets();
        IsMantling = true;
    }
    public void OnEndMantling()
    {
        IsMantling = false;
        Controller.enabled = true;
    }
    public bool CanMantling()
    {
        if (bCanMantling)
        {
            StartCoroutine(MantlingRoutine());
            return true;
        }
        return false;
    }
    private IEnumerator MantlingRoutine()
    {
        // Animation.SetMantling();
        Controller.enabled = false;
        Vector3 StartPosition = transform.position;
        float duration = 1.75f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Vector3 NextPosition = Vector3.Lerp(StartPosition, MantlingTargetPosition, t);
            transform.position = NextPosition;
            yield return null;
        }
        transform.position = MantlingTargetPosition; // + Vector3.up * 5.0f;
        Controller.enabled = true;
        bCanMantling = false;
    }
    void CalculateHandTargets()
    {
        // 벽 기준 좌우 방향
        Vector3 right = Vector3.Cross(Vector3.up, WallNormal).normalized;
        
        float handOffset = -1.0f;

        LeftHandTarget = MantlingTargetPosition - right * handOffset;
        RightHandTarget = MantlingTargetPosition + right * handOffset;

        // 벽에서 살짝 띄우기 (박힘 방지)
        LeftHandTarget += WallNormal * 0.05f;
        RightHandTarget += WallNormal * 0.05f;

        // 디버그
        DebugExtension.DrawSphere(LeftHandTarget, 0.2f, Color.blue, 1f);
        DebugExtension.DrawSphere(RightHandTarget, 0.2f, Color.red, 1f);

        Vector3 handCenter = (LeftHandTarget + RightHandTarget) * 0.5f;
        Vector3 offset = MantlingTargetPosition - handCenter;

        transform.position += offset;
    }
    private void LedgeCheck()
    {
        if (IsMantling)
        {
            return;
        }
        bCanMantling = false;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit WallHit, ForwardDistance))
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * ForwardDistance, Color.green);
            if (!WallHit.collider.CompareTag("ClimbableWall"))
            {
                return;
            }
            WallNormal = WallHit.normal;
            Vector3 PlanCheckRayStartPosition = WallHit.point + Vector3.up * MantleHeight - WallHit.normal * 1.5f;
            DebugExtension.DrawSphere(PlanCheckRayStartPosition, 1.5f, Color.red, 0.25f);

            if (Physics.Raycast(PlanCheckRayStartPosition, Vector3.down, out RaycastHit LedgeHit, LedgeCheckDistance))
            {
                Debug.DrawLine(PlanCheckRayStartPosition, PlanCheckRayStartPosition + Vector3.down * LedgeCheckDistance, Color.blue);
                bCanMantling = true;
                MantlingTargetPosition = LedgeHit.point;
                DebugExtension.DrawSphere(MantlingTargetPosition, 1.5f, Color.orange, 0.25f);
            }
            else
            {
                Debug.Log("Can't find Plane to Mantle");
            }
        }
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