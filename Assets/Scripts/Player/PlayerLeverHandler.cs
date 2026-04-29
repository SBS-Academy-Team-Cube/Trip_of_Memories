using UnityEngine;
using UnityEngine.Events;
public class PlayerLeverHandler : MonoBehaviour
{
    public UnityEvent<bool> OnLeverHolding;
    [SerializeField] private PlayerState State;
    private Lever TargetLever;
    public void HandleLever(GameObject Target)
    {
        transform.SetParent(Target.transform);
        Debug.Log("Hold Lever!");
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