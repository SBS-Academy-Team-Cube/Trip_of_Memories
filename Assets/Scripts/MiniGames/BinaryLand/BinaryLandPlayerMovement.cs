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

    public bool IsMoving => isMoving;

    private void Update()
    {
        DoMove();
    }
    private void DoMove()
    {
        if (!isMoving)
            return;
        transform.Translate(MoveDir * (Time.deltaTime * MoveSpeed));
        // // Stop movement when the target destination is reached
        if (Vector3.Distance(transform.position, TargetPos) < 0.1f)
        {
            transform.position = TargetPos;
            isMoving = false;
            
        }
    }

    public void TryMove(InputValue Value,bool IsMoveMirrord)
    {
        if (isMoving)
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

}
