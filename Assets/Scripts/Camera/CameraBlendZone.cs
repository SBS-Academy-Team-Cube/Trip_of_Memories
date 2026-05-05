using Unity.Cinemachine;
using UnityEngine;

public class CameraBlendZone : MonoBehaviour
{
    [SerializeField] private CinemachineCamera Camera;
    void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            Camera.Priority = 5;
        }
    }
    void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag("Player"))
        {
            Camera.Priority = 0;
        }
    }
}
