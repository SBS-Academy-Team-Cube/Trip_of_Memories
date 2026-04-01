using System.Runtime.CompilerServices;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    private Rigidbody rb;
    private HingeJoint joint;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }
}
