using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerInteraction Interaction;
    [SerializeField] private PlayerMovement Movement;
    [SerializeField] private PlayerRopeHandler RopeHandler;
    [SerializeField] private PlayerInput Input;
    public void OnHangingRope(bool bHanging)
    {
        Input.SwitchCurrentActionMap(bHanging ? "Hang" : "Player");
    }
    public void OnHoldLever(bool bHolding)
    {
        Input.SwitchCurrentActionMap(bHolding ? "Lever" : "Player");
    }
    public void OnInteract(InputValue Value)
    {
        if (Value.isPressed)
        {
            if (Interaction)
            {
                Interaction.PerformInteraction();
            }
        }
    }
    public void OnMove(InputValue Value)
    {
        if (Movement != null)
        {
            Movement.TryMove(Value.Get<Vector2>());
        }
    }
    public void OnJump(InputValue Value)
    {
        if (Movement != null && Value.isPressed)
        {
            Movement.TryJump();
        }
    }
    public void OnLook(InputValue Value)
    {
        if (Movement != null)
        {
            Movement.TryLook(Value);
        }
    }
    public void OnHangMove(InputValue Value)
    {
        if (Movement != null)
        {
            Movement.TryMove(Value.Get<Vector2>());
        }
    }
    public void OnHangJump(InputValue Value)
    {
        if (RopeHandler && Movement && Value.isPressed)
        {
            Movement.TryJump();
            RopeHandler.ReleaseRope();
        }
    }

    public void OnPush(InputValue Value)
    {
        if (Value.isPressed)
        {
            if (TryGetComponent(out PlayerLeverHandler Handler))
            {
                Handler.SetRotating(true);
            }
        }
        else
        {
            if (TryGetComponent(out PlayerLeverHandler Handler))
            {
                Debug.Log("Stop Pushing");
                Handler.SetRotating(false);
            }
        }
    }
}
