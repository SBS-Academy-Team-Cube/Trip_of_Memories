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
        transform.position = Target.GetPosition();
        transform.localRotation = Quaternion.Euler(0f, Target.bClockwise ? 0f : 180.0f, 0f);
        if (State)
        {
            State.SetInterAction(PlayerState.EInterAction.Pushing);
        }
        Target.TryGetComponent(out TargetLever);

        Animation.SetLeverPush(true);

        Target.GetIKPosition(out Transform Left, out Transform Right);
        Animation.SetHandIKTargets(Left, Right);
        Animation.SetHandIKWeight(1.0f, 1.0f);

        OnLeverHolding?.Invoke(true);
    }
    public void ReleaseLever()
    {
        if (TargetLever != null)
        {
            TargetLever.Release();
            TargetLever = null;
        }
        transform.SetParent(null);
        if (State)
        {
            State.SetInterAction(PlayerState.EInterAction.None);
        }
        Animation.SetLeverPush(false);
        Animation.ClearHandIK();
    }
    public void SetRotating(bool bRotate)
    {
        if (TargetLever)
        {
            TargetLever.SetIsRotating(bRotate);
        }
        Animation.SetLeverPlaying(bRotate ? 1.0f : 0.0f);
    }
}