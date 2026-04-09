using UnityEngine;

public class RoroStatue : MonoBehaviour
{
    [SerializeField] private Transform AttackTransform;
    // Cast a raycast in the direction the object is facing (or use a laser-style method...?)
    private void Update()
    {
        DoFire();
    }
    private void DoFire()
    {
        // Fire a laser until it hits an obstacle.
    }
}
