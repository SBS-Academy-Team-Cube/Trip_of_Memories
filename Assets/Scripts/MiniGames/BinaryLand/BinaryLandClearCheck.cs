using UnityEngine;

public class BinaryLandClearCheck : MonoBehaviour
{
    private bool goalCheck = false;

    public bool GoalCheck => goalCheck;

    private void Awake()
    {
        goalCheck = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MiniGameClear"))
            goalCheck = true;
        Debug.Log("clearCheck = true;");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MiniGameClear"))
            goalCheck = false;
        Debug.Log("clearCheck = false;");
    }

}
