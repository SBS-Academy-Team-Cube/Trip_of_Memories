using UnityEngine;
using Unity.Cinemachine;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform MainCameraTransform;
    [SerializeField] private SpringArm SpringArm;
    [SerializeField] private CinemachineCamera CinemachineCamera;
    [SerializeField] private CinemachineInputAxisController Input;

    public Transform GetCameraTransform()
    {
        return MainCameraTransform;
    }
    public void Init(GameObject Player)
    {
        if(Player.TryGetComponent(out PlayerState State))
        {
            Transform CameraPivot = State.GetCameraPivot();
            SpringArm.SetTarget(CameraPivot);
            CinemachineCamera.Target.TrackingTarget = CameraPivot;
        }
    }
    public void LockCamera()
    {
        Input.enabled = false;
    }
    public void ReleaseCamera()
    {
        Input.enabled = true;
    }
}
