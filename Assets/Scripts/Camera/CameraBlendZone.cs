using Unity.Cinemachine;
using UnityEngine;

public class CameraBlendZone : MonoBehaviour
{
    [SerializeField] private CinemachineCamera Camera;
    [SerializeField] private Trigger Trigger;
    
    private bool bBlending = false;

    private void OnEnable()
    {
        if(Trigger != null)
        {
            Trigger.OnTriggered += HandleCameraBlending;
        }
    }
    
    private void Disable()
    {
         if(Trigger != null)
        {
            Trigger.OnTriggered -= HandleCameraBlending;
        }
    }

    public void HandleCameraBlending()
    {
        bBlending = !bBlending;
        Camera.Priority = bBlending ? 5 : 0;
    }
}
