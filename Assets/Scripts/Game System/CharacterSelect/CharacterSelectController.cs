using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class CharacterSelectController : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private InputActionReference ClickAction;
    [SerializeField] private InputActionReference CancelAction;
    [SerializeField] private Button ConfirmBtn;

    public Action<CharacterSelectable> OnCharacterSelected;
    public Action OnSelectionCanceled;
    public Action<int> OnCharacterConfirmed;
    public Action<CharacterSelectable> OnSelectableHoverChanged;

    private CharacterSelectable CurrentSelected;
    private CharacterSelectable CurrentHovered;

    void OnEnable()
    {
        ClickAction.action.Enable();
        ClickAction.action.performed += OnClick;

        CancelAction.action.Enable();
        CancelAction.action.performed += OnCancel;
    }

    void OnDisable()
    {
        ClickAction.action.performed -= OnClick;
        ClickAction.action.Disable();

        CancelAction.action.performed -= OnCancel;
        CancelAction.action.Disable();

        SetHovered(null);
    }

    private void Update()
    {
        SetHovered(GetSelectableUnderMouse());
    }

    void OnClick(InputAction.CallbackContext context)
    {
        TrySelect();
    }

    void OnCancel(InputAction.CallbackContext context)
    {
        CurrentSelected = null;
        ConfirmBtn.interactable = false;

        OnSelectionCanceled?.Invoke();
    }

    public void OnConfirmButtonClicked()
    {
        if (CurrentSelected == null)
        {
            return;
        }
        OnCharacterConfirmed?.Invoke(CurrentSelected.MyIndex);
    }
    void TrySelect()
    {
        CharacterSelectable newTarget = GetSelectableUnderMouse();
        if (newTarget != null)
        {
            CurrentSelected = newTarget;
            OnCharacterSelected?.Invoke(CurrentSelected);
            ConfirmBtn.interactable = true;
            SetHovered(null);
        }
    }

    private CharacterSelectable GetSelectableUnderMouse()
    {
        if (MainCamera == null || Mouse.current == null)
        {
            return null;
        }

        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent(out CharacterSelectable newTarget))
        {
            return newTarget;
        }

        return null;
    }

    private void SetHovered(CharacterSelectable Selectable)
    {
        if (CurrentHovered == Selectable)
        {
            return;
        }

        CurrentHovered = Selectable;
        OnSelectableHoverChanged?.Invoke(CurrentHovered);
    }
}
