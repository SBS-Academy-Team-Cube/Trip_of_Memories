using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TempPlayerMove : MonoBehaviour
{
    [Header("이동 설정 (임시 테스트용)")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;

    private Vector2 moveInput = Vector2.zero;
    private Vector3 verticalVelocity = Vector3.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("TempPlayerMove: CharacterController가 없습니다!");
        }
    }

    // ← PlayerInputController가 호출하는 함수 (이름을 TryMove로 맞춤)
    public void TryMove(InputValue value)
    {
        if (value != null)
        {
            moveInput = value.Get<Vector2>();   // WASD 입력값 저장
        }
    }

    private void Update()
    {
        if (controller == null) return;

        // 수평 이동 방향
        Vector3 moveDirection = transform.right * moveInput.x +
                                transform.forward * moveInput.y;

        moveDirection *= moveSpeed;

        // 중력 처리
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        // 실제 이동
        controller.Move((moveDirection + verticalVelocity) * Time.deltaTime);
    }

    // 나중에 필요하면 속도 변경용
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}


