using UnityEngine;
using UnityEngine.Events;
public class PlayerLeverHandler : MonoBehaviour
{
    public UnityEvent<bool> OnLeverHolding;
    [SerializeField] private PlayerState State;
    private Lever TargetLever;
    public void HandleLever(GameObject Target, bool bClockwise)
    {
        transform.SetParent(Target.transform);
        transform.localRotation = Quaternion.Euler(0f, bClockwise ? 180f : 0f, 0f);
        if (State)
        {
            State.SetAction(PlayerState.EAction.Pushing);
        }
        Target.TryGetComponent(out TargetLever);
        OnLeverHolding?.Invoke(true);
    }
    public void SetRotating(bool bRotate)
    {
        if (TargetLever)
        {
            TargetLever.SetIsRotating(bRotate);
        }
    }
}