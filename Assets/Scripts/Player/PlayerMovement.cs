using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private PlayerMantling MantlingComponent;
    [SerializeField] private PlayerRopeHandler RopeHandler;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private PlayerAnimation Animation;
    [SerializeField] private PlayerState State;
    public Transform CameraPivot;
    private Vector2 MoveInput;
    Vector3 Velocity;
    public Transform CameraTransform;
    public float MoveSpeed = 5f;
    [SerializeField] private float RotateSpeed = 20.0f;
    public float Gravity = -9.81f;
    public float JumpForce = 5f;
    public Vector2 LookInput { get; private set; }
    private bool bAddGravity = true;
    public void SetGravity(bool bUse)
    {
        bAddGravity = bUse;
    }
    void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Animation = GetComponentInChildren<PlayerAnimation>();
    }
    void Update()
    {
        DoMove();
    }
    private void DoMove()
    {
        if (MantlingComponent.IsMantling || !Controller.enabled)
        {
            return;
        }

        if (State.Action == PlayerState.EAction.Pushing)
        {

        }
        else if (State.Action == PlayerState.EAction.Hanging)
        {
            DoHangingMove();
        }
        else
        {
            if (Controller.isGrounded && Velocity.y < 0)
            {
                Velocity.y = -2f;
            }

            Vector3 CameraForward = CameraTransform.forward;
            Vector3 CameraRight = CameraTransform.right;

            CameraForward.y = 0;
            CameraRight.y = 0;

            Vector3 move = CameraForward * MoveInput.y + CameraRight * MoveInput.x;
            move = Vector3.ClampMagnitude(move, 1f);

            Controller.Move(MoveSpeed * Time.deltaTime * move);

            if (move.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    RotateSpeed * Time.deltaTime
                );
            }
            if (bAddGravity)
            {
                Velocity.y += Gravity * Time.deltaTime;
                Controller.Move(Velocity * Time.deltaTime);
            }
        }
    }
    private void DoHangingMove()
    {
        Controller.Move(RopeHandler.Speed * Time.deltaTime * Vector3.up * MoveInput.y);
        Animation.SetRopePlaying(MoveInput.y);
    }
    public void TryMove(Vector2 Value)
    {
        MoveInput = Value;
        Animation.SetSpeed(MoveInput.magnitude);
    }
    public void TryJump()
    {
        if (Controller.isGrounded || State.Action == PlayerState.EAction.Hanging)
        {
            Velocity.y = JumpForce;
            Animation.SetJump();
        }
        else if (MantlingComponent)
        {
            MantlingComponent.TryMantling();
        }
    }
    public void TryLook(InputValue Value)
    {
        LookInput = Value.Get<Vector2>();
    }
}