using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class BinaryLandPlayerMovement : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float GridSize = 1.0f;
    [SerializeField] private float MoveSpeed = 4f;

    private bool isMoving = false;
    private Vector3 TargetPos;
    private Vector3 MoveDir;

    private Rigidbody Rigidbody;

    public bool IsMoving => isMoving;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        TargetPos = transform.position;
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        DoMove();
    }
    private void DoMove()
    {
        if (!isMoving)
            return;

        float TargetPosDistance = Vector3.Distance(transform.position, TargetPos);
        if(TargetPosDistance < 0.08f)
        {
            
            transform.position = TargetPos;

            Rigidbody.angularVelocity = Vector3.zero;//
            Rigidbody.linearVelocity = Vector3.zero;

            transform.position = Snap(transform.position);
            transform.rotation = Quaternion.Euler(Snap((new Vector3
                (transform.rotation.x, transform.rotation.y, transform.rotation.z))));

            isMoving = false;
            return;
        }

        Vector3 directionToTarget = (TargetPos - transform.position).normalized;
        Rigidbody.linearVelocity = directionToTarget * MoveSpeed;
    }

    public void TryMove(InputValue Value,bool IsMoveMirrord)
    {
        if (isMoving || Rigidbody.linearVelocity.magnitude > 0.1f)
            return;

        MoveDir = Vector3.zero;
        Vector2 Input = Value.Get<Vector2>();
        SetMoveDir(Input);

        if (MoveDir == Vector3.zero)
            return;
        if (IsMoveMirrord)
            MoveDir.x = -MoveDir.x;

        // Give the goal collider a different layer so it doesn't block player movement
        LayerMask ignoreGoal = ~LayerMask.GetMask("BinaryLandClearZone");
        if (Physics.Raycast(transform.position, MoveDir, GridSize, ignoreGoal))
            return;

        // // targetPos is set one step in the movement direction
        TargetPos = transform.position + MoveDir * GridSize;
        isMoving = true;
        
    }

    private void SetMoveDir(Vector2 Input)
    {
        //Set movement direction based on player input
        float absX = Mathf.Abs(Input.x);
        float absY = Mathf.Abs(Input.y);

        if (absX > absY)
        {
            if (Input.x > 0.5f) MoveDir = Vector3.right;
            else if (Input.x < -0.5f) MoveDir = Vector3.left;
        }
        else
        {
            if (Input.y > 0.5f) MoveDir = Vector3.forward;
            else if (Input.y < -0.5f) MoveDir = Vector3.back;
        }
    }

    private Vector3 Snap(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round((vector.x * 100f)) / 100f,
            Mathf.Round((vector.y * 100f)) / 100f,
            Mathf.Round((vector.z * 100f)) / 100f);
    }
}
