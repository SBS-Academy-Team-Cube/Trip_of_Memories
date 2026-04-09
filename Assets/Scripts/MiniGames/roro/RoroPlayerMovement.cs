using UnityEngine;

public class RoroPlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float Grid = 1f;

    private RoroPushBlock CurLookBlock;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void TryMove(Vector3 moveDir)
    {
        // When WASD is pressed, rotate to face the input direction.
        // If there is a pushable block in front of the facing direction, store it in CurLookBlock.
        // If there is no pushable block in front, set CurLookBlock to null.
        // If already facing the same direction and the front tile is empty, move one tile forward.
        // If there is a laser in front, display dialogue "" and prevent movement.
        // If CurLookBlock is not null, call CurLookBlock.BlockMove(moveDir);
    }
}
