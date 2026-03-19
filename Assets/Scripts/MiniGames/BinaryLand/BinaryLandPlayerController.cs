using UnityEngine;
using UnityEngine.InputSystem;
public class BinaryLandPlayerController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private bool MoveMirrord = false;


    private BinaryLandPlayerMovement BinaryLandPlayerMovement;

    private void Awake()
    {
        if (!TryGetComponent<BinaryLandPlayerMovement>(out BinaryLandPlayerMovement))
            Debug.Log("BinaryLandController.cs - Awake()");
    }

    public void OnMove(InputValue Value)
    {
        if (BinaryLandPlayerMovement == null ||
            BinaryLandPlayerMovement.IsMoving)
            return;

        BinaryLandPlayerMovement.TryMove(Value,MoveMirrord);
    }
}
