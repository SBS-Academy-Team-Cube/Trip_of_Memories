using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class BinaryInputMng : MonoBehaviour
{
    [SerializeField] private BinaryPlayer[] players;

    private BinaryInput inputAction;

    private void OnEnable()
    {
        inputAction = new BinaryInput();
        inputAction.binary.Enable();
        inputAction.binary.Move.performed += OnMovePerformd;
    }
    private void OnDisable()
    {
        inputAction.binary.Disable();
        inputAction.binary.Move.performed -= OnMovePerformd;
    }


    private void OnMovePerformd(InputAction.CallbackContext callback)
    {
        Vector2 dir = callback.ReadValue<Vector2>();

        foreach(var player in players)
        {
            player.TryMove(dir);
        }
    }

}
