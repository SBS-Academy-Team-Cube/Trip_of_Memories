using UnityEngine;
using System.Collections;
using UnityEngine.Events;

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
    private IEnumerator MoveRoutine(Vector3 Target)
    {
        Vector3 Start = transform.localPosition;
        float time = 0f;
        while (time < Duration)
        {
            float t = time / Duration;

            transform.localPosition = Vector3.Lerp(Start, Target, t);

            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Target;
    }
}