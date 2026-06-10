using System.Collections;
using UnityEngine;

public class SimpleFocusCamera : MonoBehaviour
{
    private CharacterSelectController Controller;
    private TransformData origin;
    private TransformData target;
    [SerializeField] private Vector2 CameraPositionOffset;
    [SerializeField] private float ToTargetPositionDuration = 2.0f;
    private Coroutine moveCoroutine;

    void Awake()
    {
        Controller = FindFirstObjectByType<CharacterSelectController>();

        if (Controller)
        {
            Controller.OnCharacterSelected += SetFocusTarget;
            Controller.OnSelectionCanceled += ReturnToOrigin;
        }
        origin = new TransformData(transform.position, transform.rotation);
    }

    private void SetFocusTarget(CharacterSelectable Target)
    {
        Vector3 pos =
            Target.transform.position
            + Target.transform.forward * CameraPositionOffset.x
            + Target.transform.up * CameraPositionOffset.y;

        Quaternion rot = Quaternion.LookRotation(
            (Target.transform.position - pos).normalized
        );

        target = new TransformData(pos, rot);
        StartMove(target);
    }

    private void ReturnToOrigin()
    {
        StartMove(origin);
    }

    private void StartMove(TransformData to)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        TransformData from = new TransformData(transform.position, transform.rotation);

        moveCoroutine = StartCoroutine(Move(from, to));
    }

    private IEnumerator Move(TransformData from, TransformData to)
    {
        float time = 0f;

        while (time < ToTargetPositionDuration)
        {
            time += Time.deltaTime;
            float t = time / ToTargetPositionDuration;

            transform.SetPositionAndRotation(
                Vector3.Lerp(from.position, to.position, t),
                Quaternion.Slerp(from.rotation, to.rotation, t)
            );

            yield return null;
        }

        transform.SetPositionAndRotation(to.position, to.rotation);
    }
}

public struct TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public TransformData(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}