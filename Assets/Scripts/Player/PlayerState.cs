using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum EGait { Walking, Running };
    public enum EStance { Standing, Crouching };
    public enum EAction { None, Holding, Hanging, Pushing };

    public bool IsGrounded = true;
    public bool IntendToMove = false;
    public bool IntendToSprint = false;
    public bool IsInteracting = false;
    public EGait Gait = EGait.Walking;
    private EStance Stance = EStance.Standing;
    public EAction Action { get; private set; } = EAction.None;
    public event System.Action<EGait> OnGaitChanged;
    public event System.Action<EStance> OnStanceChanged;
    public event System.Action<EAction> OnActionChanged;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform CameraPivot;
    public Transform GetCameraPivot() 
    { 
        return CameraPivot;
    }
    void Awake()
    {
        if (Controller == null)
        {
            Controller = GetComponent<CharacterController>();
        }
    }
    void Update()
    {
        IsGrounded = Controller.isGrounded;
    }
    public void OnInteractionEnd()
    {
        IsInteracting = false;
    }

    public void TryMove(bool bWantToMove)
    {
        if (IntendToMove != bWantToMove)
        {
            IntendToMove = bWantToMove;
        }
    }
    public void TrySprint(bool bWantToSprint)
    {
        if (IntendToSprint != bWantToSprint)
        {
            IntendToSprint = bWantToSprint;
        }
    }
    public void SetGait(EGait NewGait)
    {
        if (NewGait == EGait.Running && !CanRun())
        {
            return;
        }
        // if(NewGait != EGait.Idle && !CanMove())
        // {
        //     return;
        // }
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
    private bool CanRun()
    {
        return Action == EAction.None && !IsInteracting;
    }
    public bool CanMove()
    {
        return !IsInteracting;
    }
};