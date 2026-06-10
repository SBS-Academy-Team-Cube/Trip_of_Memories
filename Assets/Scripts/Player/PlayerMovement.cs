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
    private Vector2 MoveInput;
    private Vector3 Velocity;
    public Transform CameraTransform { get; private set; }

    [Header("Settings")]
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float SprintSpeed;

    [SerializeField] private float RotateSpeed = 20.0f;
    [SerializeField] private float Gravity = -9.81f;
    [SerializeField] private float JumpForce = 5f;
    public void SetCameraTransform(Transform CameraTransform)
    {
        this.CameraTransform = CameraTransform;
    }

    void Update()
    {
        DoMove();
    }
    private void DoMove()
    {
        if (!State.CanMove() || MantlingComponent.IsMantling || !Controller.enabled)
        {
            return;
        }
        else if (State.InterAction == PlayerState.EInterAction.LeverPushing)
        {
            Velocity = Vector3.zero;
            Animation.SetIsMoving(false);
            return;
        }
        else if (State.InterAction == PlayerState.EInterAction.RopeHanging)
        {
            DoHangingMove();
        }
        else
        {
            if (Controller.isGrounded && Velocity.y < 0)
            {
                Velocity.y = -2f;
            }
            if (State.IntendToMove)
            {
                Animation.SetIsMoving(true);

                Vector3 CameraForward = CameraTransform.forward;
                Vector3 CameraRight = CameraTransform.right;

                CameraForward.y = 0;
                CameraRight.y = 0;

                Vector3 Move = CameraForward * MoveInput.y + CameraRight * MoveInput.x;
                Move = Vector3.ClampMagnitude(Move, 1f);

                Controller.Move((State.IntendToSprint && State.CanSprint() ? SprintSpeed : MoveSpeed) * Time.deltaTime * Move);
                Animation.SetGait(State.IntendToSprint && State.CanSprint() ? 1 : 0);

                if (Move.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(Move);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        RotateSpeed * Time.deltaTime
                    );
                }
            }
            else
            {
                Animation.SetIsMoving(false);
            }
            Velocity.y += Gravity * Time.deltaTime;
            Controller.Move(Velocity * Time.deltaTime);
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
    }
    public void TryJump()
    {
        if (Controller.isGrounded || State.InterAction == PlayerState.EInterAction.RopeHanging)
        {
            Velocity.y = JumpForce;
            Animation.SetJump();
        }
        else if (MantlingComponent)
        {
            MantlingComponent.TryMantling();
        }
    }
}
