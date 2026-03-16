using UnityEngine;
using Unity.Cinemachine;
using System;
public class CameraLook : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement PlayerMovement;
    [SerializeField]
    private CinemachineRotationComposer RotationComposer;
    private Vector2 ScreenOffset;
    
    [SerializeField]
    private float Sensitivity = 0.25f;

    [SerializeField]
    private Vector2 HorizontalLookOffset;   // (Leftmost, rightmost) offset values
    [SerializeField]
    private Vector2 VerticalLookOffset;     // (Bottom,top) offset values

    void Update()
    {
        if(PlayerMovement != null)
        {
            CameraRotate();
        }
    }
    private void CameraRotate()
    {
        Vector2 Look = PlayerMovement.LookInput;
        Look.x *= -1f;
        ScreenOffset += Look * Sensitivity;

        ScreenOffset.x = Mathf.Clamp(ScreenOffset.x, HorizontalLookOffset.x, HorizontalLookOffset.y);
        ScreenOffset.y = Mathf.Clamp(ScreenOffset.y, VerticalLookOffset.x, VerticalLookOffset.y);

        // ScreenPosition indicates the position on the screen where the target should be placed. Range: -0.5 to 0.5
        RotationComposer.Composition.ScreenPosition = ScreenOffset;
    }
}
