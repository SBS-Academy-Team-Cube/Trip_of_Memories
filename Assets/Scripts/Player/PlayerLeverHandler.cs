using UnityEngine;
using UnityEngine.Events;
public class PlayerLeverHandler : MonoBehaviour
{
    public UnityEvent<bool> OnLeverHolding;
    [SerializeField] private PlayerState State;
    [SerializeField] private PlayerAnimation Animation;
    private Lever TargetLever;
    public void HandleLever(Lever Target)
    {
        transform.SetParent(Target.transform);

        Vector3 TargetPosition = Target.GetPosition();
        transform.position = new Vector3(TargetPosition.x, transform.position.y, TargetPosition.z);
        transform.localRotation = Quaternion.Euler(0f, Target.bClockwise ? 0f : 180.0f, 0f);
        if (State)
        {
            State.SetAction(PlayerState.EAction.Pushing);
        }
        Target.TryGetComponent(out TargetLever);

        Animation.SetLeverPush(true);

        Target.GetIKPosition(out Transform Left, out Transform Right);
        Animation.SetHandIKTargets(Left, Right);
        Animation.SetHandIKWeight(1.0f, 1.0f);

        OnLeverHolding?.Invoke(true);
    }
    public void SetRotating(bool bRotate)
    {
        if (TargetLever)
        {
            TargetLever.SetIsRotating(bRotate);
        }
        Animation.SetLeverPlaying(bRotate);
    }
}