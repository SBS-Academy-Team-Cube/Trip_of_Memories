using UnityEngine;
using UnityEngine.InputSystem;

public class RoroPlayerInput : MonoBehaviour
{
    private RoroInput InputAction;

    private RoroPlayerMovement Movement;
    private RoroPlayerAttack Attack;

    private void Awake()
    {
        InputAction = new RoroInput();
        Movement = GetComponent<RoroPlayerMovement>();
        Attack = GetComponent<RoroPlayerAttack>();
    }
    private void OnEnable()
    {
        InputAction.Enable();
        InputAction.Roro.Move.started += OnMove;
        InputAction.Roro.Attack.performed += OnAttack;
    }
    private void OnDisable()
    {
        InputAction.Disable();
        InputAction.Roro.Move.started -= OnMove;
        InputAction.Roro.Attack.performed -= OnAttack;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        // When WASD keys are pressed, rotate to face the corresponding direction.
        // If already facing that direction, move one tile forward in that direction.
        // If a pushable block is blocking the way, push the block and move together with it.
        Vector2 input = ctx.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x,0f,input.y);
        Movement.TryMove(moveDir);
    }
    private void OnAttack(InputAction.CallbackContext ctx)
    {
        // When the E key is pressed, perform an attack in the 1 tile forward direction the player is currently facing.
        Attack.TryAttack();
    }
}
