using UnityEngine;

public class RoroPushBlock : MonoBehaviour
{
    // Called when the block's parent needs to be changed in the hierarchy
    public void PushBlockWithPlayerAsParent(GameObject player)
    {
        transform.SetParent(player.transform);
    }
    public void DetachBlockFromPlayer()
    {
        transform.SetParent(null);
    }
}
