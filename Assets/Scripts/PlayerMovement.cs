using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController Controller;
    PlayerAnimation Animation;
    Vector2 MoveInput;
    Vector3 Velocity;

    public Transform CameraTransform;
    public Transform CameraPivot;

    public float MoveSpeed = 5f;
    [SerializeField]
    private float RotateSpeed = 20.0f;
    public float Gravity = -9.81f;
    public float JumpForce = 5f;
    public Vector2 LookInput { get; private set; }

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

        Velocity.y += Gravity * Time.deltaTime;
        Controller.Move(Velocity * Time.deltaTime);
    }

    public void OnMove(InputValue Value)
    {
        MoveInput = Value.Get<Vector2>();
        Animation.SetSpeed(MoveInput.magnitude);
    }

    public void OnJump(InputValue Value)
    {
        if (Value.isPressed && Controller.isGrounded)
        {
            Velocity.y = JumpForce;
            Animation.SetJump(true);
        }
    }
    public void OnLook(InputValue Value)
    {
        LookInput = Value.Get<Vector2>();
    }
}