using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum EGait { Walking, Running };
    public enum EStance { Standing, Crouching };
    public enum EAction { None, Holding, Hanging, Pushing };
    private EGait Gait = EGait.Walking;
    private EStance Stance = EStance.Standing;
    public EAction Action { get; private set; } = EAction.None;
    public event System.Action<EGait> OnGaitChanged;
    public event System.Action<EStance> OnStanceChanged;
    public event System.Action<EAction> OnActionChanged;

    public void SetGait(EGait NewGait)
    {
        if (NewGait != Gait)
        {
            Gait = NewGait;
            OnGaitChanged?.Invoke(Gait);
        }
    }
    public void SetStance(EStance NewStance)
    {
        if (NewStance != Stance)
        {
            Stance = NewStance;
            OnStanceChanged?.Invoke(Stance);
        }
    }
    public void SetAction(EAction NewAction)
    {
        if (NewAction != Action)
        {
            Action = NewAction;
            OnActionChanged?.Invoke(Action);
        }
    }
};