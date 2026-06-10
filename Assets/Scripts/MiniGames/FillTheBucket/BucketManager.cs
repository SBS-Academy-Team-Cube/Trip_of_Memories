using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class BucketManager : MonoBehaviour
{
    [Header("Bucket")]
    [SerializeField] private Bucket Bucket_3L;
    [SerializeField] private Bucket Bucket_7L;
    [SerializeField] private Bucket Bucket_5L;
    [SerializeField] private BucketUIManager UI;

    [Header("UI Input")]
    [SerializeField] private InputActionReference CancelAction;
    private Bucket SourceBucket = null;
    private Bucket TargetBucket = null;
    public void Init()
    {
        Bucket_3L.Reset();
        Bucket_7L.Reset();
        Bucket_5L.Reset();
    }
    public void Reset()
    {
        if (SourceBucket != null)
        {
            SourceBucket.PickDownBucket();
            SourceBucket = null;
        }
        Init();
    }
    private void OnEnable()
    {
        CancelAction.action.performed += OnCancel;
        CancelAction.action.Enable();
    }
    private void OnDisable()
    {
        CancelAction.action.performed -= OnCancel;
        CancelAction.action.Disable();
    }
    public void OnBucketSelected(Bucket SelectedBucket)
    {
        if (SourceBucket == null)
        {
            SourceBucket = SelectedBucket;
            SourceBucket.PickUpBucket();
        }
        else if (TargetBucket == null)
        {
            TargetBucket = SelectedBucket;
            if (SourceBucket.CurrentAmount == 0)
            {
                UI.ShowPopup("옮길 양동이가 비어있습니다!");
                TargetBucket.EnableButton();
                TargetBucket = null;
                SourceBucket.PickDownBucket();
                SourceBucket = null;
                return;
            }
            else if (!TargetBucket.CanFill(SourceBucket.CurrentAmount))
            {
                UI.ShowPopup("해당 양동이에는 물을 채울 수 없습니다!");
                TargetBucket.EnableButton();
                TargetBucket = null;
            }
            else
            {
                SourceBucket.DoPouring(TargetBucket);
                SourceBucket = null;
                TargetBucket = null;
            }
        }
    }
    private void OnCancel(InputAction.CallbackContext Context)
    {
        if (SourceBucket != null)
        {
            SourceBucket.PickDownBucket();
            SourceBucket = null;
        }
    }
    public void FillBucket()
    {
        if (Bucket_3L != null && Bucket_3L.CurrentAmount != 3)
        {
            Bucket_3L.Fill(3);
        }
    }
}
