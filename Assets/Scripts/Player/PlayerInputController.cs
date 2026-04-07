using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInteraction Interaction;
    private PlayerMovement Movement;

    private void Awake()
    {
        if (!TryGetComponent<PlayerInteraction>(out Interaction))
        {
            Debug.Log("PlayerInputController.cs - Awake() - interaction component not found");
        }
        if (!TryGetComponent<PlayerMovement>(out Movement))
        {
            Debug.Log("PlayerInputController.cs - Awake() - movement component not found");
        }
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
            Movement.TryMove(Value);
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
}
