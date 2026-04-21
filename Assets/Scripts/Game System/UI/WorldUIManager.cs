using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class WorldUIManager : MonoBehaviour
{
    [SerializeField] GameObject TextUIObject;
    [SerializeField] TMP_Text UIText;
    [SerializeField] Transform CameraTransform;
    [SerializeField] Vector3 UIOffset = new Vector3(1f, 1f, 0f);
    private Transform UITargetTransfrom;
    private PlayerInteraction PlayerInteractionComponent;
    private bool bShowing = false;
    public void SetPlayerInteraction(PlayerInteraction PlayerInteractionComponent)
    {
        this.PlayerInteractionComponent = PlayerInteractionComponent;
        if (this.PlayerInteractionComponent != null)
        {
            PlayerInteractionComponent.OnTargetChanged += ShowWorldUIText;
        }
    }
    private void OnDisable()
    {
        if (PlayerInteractionComponent != null)
        {
            PlayerInteractionComponent.OnTargetChanged -= ShowWorldUIText;
        }
    }
    public void ShowWorldUIText(string Text, Transform TargetTransform, bool bShowing)
    {
        this.bShowing = bShowing;
        if (!bShowing)
        {
            TextUIObject.SetActive(false);
            return;
        }
        TextUIObject.SetActive(true);
        UIText.text = Text;
        UITargetTransfrom = TargetTransform;
    }
    private void LateUpdate()
    {
        if (CameraTransform == null || !bShowing)
        {
            return;
        }
        Vector3 CamRight = CameraTransform.right;
        Vector3 CamUp = CameraTransform.up;

        Vector3 Offset = CamRight * UIOffset.x + CamUp * UIOffset.y;

        transform.position = UITargetTransfrom.position + Offset;

        transform.forward = CameraTransform.forward;
    }
}
