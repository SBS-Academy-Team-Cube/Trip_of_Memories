using Unity.Cinemachine;
using UnityEngine;

public class CameraBlendZone : MonoBehaviour
{
    [SerializeField] private CinemachineCamera Camera;
    [SerializeField] private Lever TargetLever = null;
    
    private void OnEnable()
    {
        if(TargetLever != null)
        {
            TargetLever.OnLeverInteracted += HandleCameraBlending;
        }
    }

    private void Disable()
    {
         if(TargetLever != null)
        {
            TargetLever.OnLeverInteracted -= HandleCameraBlending;
        }
    }

    public void HandleCameraBlending(bool bBlend)
    {
        Camera.Priority = bBlend ? 5 : 0;
    }
}
