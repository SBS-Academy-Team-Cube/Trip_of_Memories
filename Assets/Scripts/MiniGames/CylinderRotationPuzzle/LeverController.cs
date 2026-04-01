
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] private float moterSpeed = 0.5f;
    private bool isMoter = false;

    private void Update()
    {
        if(isMoter)
        {
            transform.Rotate(-Vector3.up * (moterSpeed * Time.deltaTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("OnTrigger!");
            isMoter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isMoter = false;
        }    
    }
}
