using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class BinaryInputMng : MonoBehaviour
{
    [SerializeField] private BinaryPlayer[] players;
    [SerializeField] private BinaryUIMng uiMng;

    private BinaryInput inputAction;

    public void GameStart()
    {
        inputAction = new BinaryInput();
        inputAction.binary.Enable();
        inputAction.binary.Move.performed += OnMovePerformd;
    }
    public void GameExit()
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

        uiMng.ClearCheck();
    }


}
