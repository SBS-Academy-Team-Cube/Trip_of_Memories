using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private InputActionReference ClickAction;
    [SerializeField] private InputActionReference CancelAction;
    public System.Action<CharacterSelectable> OnCharacterSelected;
    private CharacterSelectable CurrentSelected;
    public System.Action OnSelectionCanceled;
    [SerializeField] private Button ConfirmBtn;
    void OnEnable()
    {
        ClickAction.action.Enable();
        ClickAction.action.performed += OnClick;

        CancelAction.action.Enable();
        CancelAction.action.performed += OnCancel;

        // ConfirmBtn.onClick.AddListener(OnSelected);
    }
    void OnDisable()
    {
        ClickAction.action.performed -= OnClick;
        ClickAction.action.Disable();

        CancelAction.action.performed -= OnCancel;
        CancelAction.action.Disable();

        // ConfirmBtn.onClick.RemoveListener(OnSelected);

    }

    void OnClick(InputAction.CallbackContext Context)
    {
        TrySelect();
    }
    void OnCancel(InputAction.CallbackContext Context)
    {
        OnSelectionCanceled?.Invoke();
        ConfirmBtn.interactable = false;
    }
    public void OnSelected()
    {
        UIEventBus.OnAnyButtonClicked?.Invoke();
        if (CurrentSelected == null)
        {
            Debug.Log("There is No Selected Character");
            return;
        }
        if(SaveManager.Instance == null || GameDirector.Instance == null)
        {
            Debug.Log("SaveManager OR GameDirector Instance is NULL");
            return;
        }
        SaveManager.Instance.Data.SelectedCharacterModelIndex = CurrentSelected.MyIndex;
        SaveManager.Instance.Data.HasPlayed = true;
        SaveManager.Instance.Save();
        GameDirector.Instance.LoadScene(3);
    }
    void TrySelect()
    {
        Ray Ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(Ray, out RaycastHit HitResult))
        {
            if (HitResult.collider.TryGetComponent(out CharacterSelectable NewTarget))
            {
                CurrentSelected = NewTarget;
                OnCharacterSelected?.Invoke(CurrentSelected);
                ConfirmBtn.interactable = true;
            }
        }
    }
}
