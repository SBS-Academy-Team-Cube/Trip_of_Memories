using UnityEngine;
using System.Collections;

public class PlayerMantling : MonoBehaviour
{
    [Header("Mantle Settings")]
    public float MantleHeight = 1.5f;
    public float MantleForwardDistance = 1.0f;
    public float MantleSpeed = 5f;
    public float ForwardDistance = 1.0f;
    public float LedgeCheckDistance = 10.0f;

    [SerializeField] private Transform LedgeCheckPosition;
    private CharacterController Controller;
    private bool IsMantling;
    private Vector3 TargetPosition;

    private bool bCanMantling = false;
    Vector3 MantlingTargetPosition;
    void Awake()
    {
        Controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        LedgeCheck();
    }
    public bool CanMantling()
    {
        if (bCanMantling)
        {
            StartCoroutine(DoMantling());
            return true;
        }
        return false;
    }
    private IEnumerator DoMantling()
    {
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
    private void LedgeCheck()
    {
        if (IsMantling)
        {
            return;
        }
        bCanMantling = false;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit WallHit, ForwardDistance))
        {
            if (!WallHit.collider.CompareTag("ClimbableWall"))
            {
                return;
            }
            Debug.DrawLine(WallHit.point, WallHit.normal * 5.0f, Color.green);
            Vector3 PlanCheckRayStartPosition = WallHit.point + Vector3.up * MantleHeight - WallHit.normal * 1.5f;
            if (Physics.Raycast(PlanCheckRayStartPosition, Vector3.down, out RaycastHit LedgeHit, LedgeCheckDistance))
            {
                Debug.DrawLine(PlanCheckRayStartPosition, Vector3.down * LedgeCheckDistance, Color.blue);
                bCanMantling = true;
                MantlingTargetPosition = LedgeHit.point;
                DebugExtension.DrawSphere(MantlingTargetPosition, 3.0f, Color.red, 0.1f);
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