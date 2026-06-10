using UnityEngine;
using System;

public class PlayerState : MonoBehaviour
{
    [Header("Components Reference")]
    [SerializeField] private PlayerInteraction InteractionComponent;
    [Header("Interaction Components")]
    [SerializeField] private PlayerRopeHandler RopeHandler;
    [SerializeField] private PlayerItemHandler ItemHandler;
    [SerializeField] private PlayerLeverHandler LeverHandler;

    public enum EGait { Walking, Running };
    public enum EStance { Standing, Crouching };
    public enum EInterAction { None, ItemHolding, RopeHanging, LeverPushing };
    public enum EAbility { None, Spray, Lantern };
    public EGait Gait { get; private set; } = EGait.Walking;
    public EStance Stance { get; private set; } = EStance.Standing;
    public EInterAction InterAction { get; private set; } = EInterAction.None;
    public EAbility Ability { get; private set; } = EAbility.None;
    public bool IsGrounded { get; private set; }
    public bool IntendToMove = false;
    public bool IntendToSprint = false;
    public bool IsInteracting = false;

    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform CameraPivot;
    public Transform GetCameraPivot()
    {
        return CameraPivot;
    }
    public void TryInteraction()
    {
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
    public void SetStance(EStance NewStance)
    {
        if (NewStance != Stance)
        {
            Stance = NewStance;
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
        return InterAction == EInterAction.None && CanMove() && Stance == EStance.Standing;
    }
    public bool CanMove()
    {
        return !IsInteracting;
    }
};