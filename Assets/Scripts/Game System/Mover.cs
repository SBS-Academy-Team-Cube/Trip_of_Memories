using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    [SerializeField] Vector3 LocalOffset;
    [SerializeField] float Duration = 1.0f;
    private Vector3 StartPosition;
    private Vector3 EndPosition;
    private Vector3 TargetPosition;
    private Coroutine CurrentMoveRoutine;

    private void Awake()
    {
        StartPosition = transform.localPosition;
        EndPosition = StartPosition + LocalOffset;
        TargetPosition = EndPosition;
    }
    public void Move()
    {
        if (CurrentMoveRoutine != null)
        {
            StopCoroutine(CurrentMoveRoutine);
        }
        CurrentMoveRoutine = StartCoroutine(MoveRoutine(TargetPosition));
        TargetPosition = TargetPosition == EndPosition ? StartPosition : EndPosition;
    }

    private float GetCurrentMoveRatio()
    {
        Vector3 segment = EndPosition - StartPosition;

        if (segment.sqrMagnitude <= Mathf.Epsilon)
        {
            return 0f;
        }

        float ratio = Vector3.Dot(transform.localPosition - StartPosition, segment) / segment.sqrMagnitude;
        return Mathf.Clamp01(ratio);
    }

    private IEnumerator MoveRoutine(Vector3 Target)
    {
        if (Duration <= Mathf.Epsilon)
        {
            transform.localPosition = Target;
            CurrentMoveRoutine = null;
            yield break;
        }

        float currentRatio = GetCurrentMoveRatio();
        float targetRatio = Target == EndPosition ? 1f : 0f;
        float moveSpeed = 1f / Duration;

        while (!Mathf.Approximately(currentRatio, targetRatio))
        {
            currentRatio = Mathf.MoveTowards(currentRatio, targetRatio, moveSpeed * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, currentRatio);
            yield return null;
        }

        transform.localPosition = Target;
        CurrentMoveRoutine = null;
    }
}
