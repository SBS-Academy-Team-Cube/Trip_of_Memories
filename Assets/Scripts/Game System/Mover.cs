using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    [SerializeField] Vector3 LocalOffset;
    [SerializeField] float Duration = 1.0f;

    public UnityEvent OnMoveStart;
    public UnityEvent OnMoveEnd;

    private Vector3 ClosedPosition;
    private Vector3 OpenPosition;
    private Coroutine CurrentMoveRoutine;

    private void Awake()
    {
        ClosedPosition = transform.localPosition;
        OpenPosition = ClosedPosition + LocalOffset;
    }
    public void Open()
    {
        StartMove(OpenPosition);
    }
    public void Close()
    {
        StartMove(ClosedPosition);
    }
    private void StartMove(Vector3 Target)
    {
        if (CurrentMoveRoutine != null)
        {
            StopCoroutine(CurrentMoveRoutine);
        }
        CurrentMoveRoutine = StartCoroutine(MoveRoutine(Target));
        OnMoveStart?.Invoke();
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
        OnMoveEnd?.Invoke();
    }
}