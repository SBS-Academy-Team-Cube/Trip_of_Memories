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
        if (Movement)
        {
            Movement.enabled = !bHanging;
        }
        Input.SwitchCurrentActionMap(bHanging ? "Hang" : "Player");
    }
    public void OnInteract(InputValue Value)
    {
        if (Value.isPressed)
        {
            if (Interaction != null)
            {
                Interaction.PerformInteraction();
                Debug.Log("press E");
            }
            else
            {
                Debug.Log("Interaction Key is Downed");
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
        if (Movement != null)
        {
            Movement.TryJump(Value);
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
        if (RopeHandler)
        {
            RopeHandler.TryMove(Value.Get<Vector2>());
        }
    }
}
