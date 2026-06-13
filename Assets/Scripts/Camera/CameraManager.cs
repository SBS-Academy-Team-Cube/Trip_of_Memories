using UnityEngine;
using Unity.Cinemachine;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform MainCameraTransform;
    [SerializeField] private SpringArm SpringArm;
    [SerializeField] private CinemachineCamera FreeLookCamera;
    [SerializeField] private CinemachineCamera TopViewCamera;
    [SerializeField] private CinemachineInputAxisController Input;
    public event System.Action OnInitialized;
    private PlayerMovement PlayerMoveComponent;
    public Transform GetCameraTransform()
    {
        return MainCameraTransform;
    }
    public void Init(GameObject Player)
    {
        if (Player.TryGetComponent(out PlayerState State))
        {
            Transform CameraPivot = State.GetCameraPivot();
            SpringArm.SetTarget(CameraPivot);
            FreeLookCamera.Target.TrackingTarget = CameraPivot;
            TopViewCamera.Target.TrackingTarget = CameraPivot;
        }
        Player.TryGetComponent(out PlayerMoveComponent);
        OnInitialized?.Invoke();
    }
    public void ChangeToTopViewCam()
    {
        if (PlayerMoveComponent != null)
        {
            PlayerMoveComponent.ChangeCameraView(false);
        }
        FreeLookCamera.Priority = 2;
        TopViewCamera.Priority = 5;
    }
    public void ChangeToFreeLookCam()
    {
        if (PlayerMoveComponent != null)
        {
            PlayerMoveComponent.ChangeCameraView(true);
        }
        TopViewCamera.Priority = 2;
        FreeLookCamera.Priority = 5;
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
