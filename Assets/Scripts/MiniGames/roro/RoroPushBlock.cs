using UnityEngine;

public class RoroPushBlock : MonoBehaviour
{
    // If the player pushes this block, it moves one tile in that direction together with the player.
    // When the player performs a move action, if there is a pushable block in front, call the push function.
    public void BlockMove(Vector3 MoveDir)
    {
        // If this function is called, the object is pushed one tile in the moveDir direction (if no obstacle in front).
    }
}
