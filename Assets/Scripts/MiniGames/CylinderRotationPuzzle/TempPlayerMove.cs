using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TempPlayerMove : MonoBehaviour
{
    [Header("�̵� ���� (�ӽ� �׽�Ʈ��)")]
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
            Debug.LogError("TempPlayerMove: CharacterController�� �����ϴ�!");
        }
    }

    // �� PlayerInputController�� ȣ���ϴ� �Լ� (�̸��� TryMove�� ����)
    public void TryMove(InputValue value)
    {
        if (value != null)
        {
            moveInput = value.Get<Vector2>();   // WASD �Է°� ����
        }
    }

    private void Update()
    {
        if (controller == null) return;

        // ���� �̵� ����
        Vector3 moveDirection = transform.right * moveInput.x +
                                transform.forward * moveInput.y;

        moveDirection *= moveSpeed;

        // �߷� ó��
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        // ���� �̵�
        controller.Move((moveDirection + verticalVelocity) * Time.deltaTime);
    }

    // ���߿� �ʿ��ϸ� �ӵ� �����
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}


