using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Components Reference")]
    [SerializeField] private PlayerInteraction InteractionComponent;
    [Header("Interaction Components")]
    [SerializeField] private PlayerRopeHandler RopeHandler;
    [SerializeField] private PlayerItemHandler ItemHandler;
    [SerializeField] private PlayerLeverHandler LeverHandler;
    [SerializeField] private PlayerDragHandler DragHandler;
    [SerializeField] private PlayerAnimation AnimationComponent;
    [SerializeField] private Health HP;
    public enum EGait { Walking, Running };
    public enum EStance { Standing, Crouching };
    public enum EInterAction { None, ItemHolding, RopeHanging, LeverPushing, KeyDragging };
    public enum EAbility { None, Spray, Lantern };
    public EGait Gait { get; private set; } = EGait.Walking;
    public EStance Stance { get; private set; } = EStance.Standing;
    public EInterAction InterAction { get; private set; } = EInterAction.None;
    public EAbility Ability { get; private set; } = EAbility.None;
    public bool IsGrounded { get; private set; }
    public bool IntendToMove = false;
    public bool IntendToSprint = false;
    public bool IsInteracting = false;
    private bool IsAlive = true;
    public event System.Action OnDeathEnd;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private float CrouchRatio;
    [SerializeField] private LayerMask StandUpBlockLayerMask = ~0;
    [SerializeField] private Transform CameraPivot;
    private const float StandUpCheckSkin = 0.02f;

    private void OnEnable()
    {
        HP.OnDead += OnDead;
    }
    private void OnDisable()
    {
        HP.OnDead -= OnDead;
    }
    private void OnDead()
    {
        IsAlive = false;
        if (ItemHandler.bIsHoldingItem)
        {
            ItemHandler.DropItem();
        }
        AnimationComponent.SetDeath();
    }
    private void HandleDeathEnd()
    {
        OnDeathEnd?.Invoke();
    }

    public Transform GetCameraPivot()
    {
        return CameraPivot;
    }
    public void TryChangeStance()
    {
        if (!IsGrounded || Gait == EGait.Running || InterAction != EInterAction.None || !IsAlive)
        {
            return;
        }

        EStance NewStance = Stance == EStance.Standing ? EStance.Crouching : EStance.Standing;
        if (Stance == EStance.Crouching && NewStance == EStance.Standing && !CanStandUp())
        {
            return;
        }
        SetStance(NewStance);

    }
    public void TryTakeLantern()
    {
        if (!IsGrounded || Gait == EGait.Running || InterAction != EInterAction.None || Ability == EAbility.Spray || !IsAlive)
        {
            return;
        }
        SetAbility(Ability == EAbility.Lantern ? EAbility.None : EAbility.Lantern);
        AnimationComponent.SetTakeLantern();
    }
    public void TryInteraction()
    {
        if (!IsAlive)
        {
            return;
        }
        if (Ability != EAbility.None)
        {
            return;
        }
        switch (InterAction)
        {
            case EInterAction.ItemHolding:
                if (ItemHandler != null)
                {
                    ItemHandler.TryDrop();
                }
                break;
            case EInterAction.LeverPushing:
                if (LeverHandler != null)
                {
                    LeverHandler.ReleaseLever();
                }
                break;
            case EInterAction.None:
                if (InteractionComponent != null)
                {
                    InteractionComponent.PerformInteraction();
                }
                return;
        }
    }
    public void TryEscape()
    {
        switch (InterAction)
        {
            case EInterAction.LeverPushing:
                LeverHandler.ReleaseLever();
                break;
            case EInterAction.KeyDragging:
                DragHandler.TryRelease();
                break;
        }
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
        if (!IsAlive)
        {
            return;
        }
        if (IntendToMove != bWantToMove)
        {
            IntendToMove = bWantToMove;
        }
    }
    public void TrySprint(bool bWantToSprint)
    {
        if (!IsAlive)
        {
            return;
        }
        if (IntendToSprint != bWantToSprint)
        {
            IntendToSprint = bWantToSprint;
            SetGait(IntendToSprint ? EGait.Running : EGait.Walking);
        }
    }
    public void SetStance(EStance NewStance)
    {
        if (!IsAlive)
        {
            return;
        }
        if (NewStance != Stance)
        {
            Stance = NewStance;
            Controller.height *= CrouchRatio;
            Controller.center *= CrouchRatio;
            CrouchRatio = 1.0f / CrouchRatio;
            AnimationComponent.SetStance((int)Stance);
        }
    }
    private bool CanStandUp()
    {
        if (Controller == null || !IsAlive)
        {
            return true;
        }

        float StandHeight = Controller.height * CrouchRatio;
        Vector3 StandCenter = Controller.center * CrouchRatio;
        Vector3 WorldCenter = transform.TransformPoint(StandCenter);
        Vector3 Up = transform.up;

        float HeightScale = Mathf.Abs(transform.lossyScale.y);
        float RadiusScale = Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.z));
        float Radius = Mathf.Max(0.0f, Controller.radius * RadiusScale - StandUpCheckSkin);
        float HalfHeight = Mathf.Max(Radius, StandHeight * HeightScale * 0.5f);
        float CapsuleOffset = Mathf.Max(0.0f, HalfHeight - Radius);

        Vector3 Top = WorldCenter + Up * CapsuleOffset;
        Vector3 Bottom = WorldCenter - Up * CapsuleOffset;
        float CurrentTopHeight = Controller.bounds.max.y;

        Collider[] Hits = Physics.OverlapCapsule(Bottom, Top, Radius, StandUpBlockLayerMask, QueryTriggerInteraction.Ignore);
        foreach (Collider Hit in Hits)
        {
            if (Hit == null || Hit == Controller || Hit.transform.IsChildOf(transform))
            {
                continue;
            }

            if (Hit.bounds.max.y <= CurrentTopHeight + StandUpCheckSkin)
            {
                continue;
            }
            return false;
        }

        return true;
    }
    private void SetGait(EGait NewGait)
    {
        if (Gait != NewGait)
        {
            if (NewGait == EGait.Running && !CanSprint())
            {
                return;
            }
            Gait = NewGait;
            AnimationComponent.SetGait((int)Gait);
        }
    }
    public void SetInterAction(EInterAction NewAction)
    {
        if (NewAction != InterAction)
        {
            InterAction = NewAction;
        }
    }
    public void SetAbility(EAbility NewAbility)
    {
        if (NewAbility != Ability)
        {
            Ability = NewAbility;
        }
    }
    public bool CanSprint()
    {
        return InterAction == EInterAction.None && CanMove() && Stance == EStance.Standing && IsGrounded && IsAlive;
    }
    public bool CanMove()
    {
        return !IsInteracting && IsAlive;
    }
};
