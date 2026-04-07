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

    private CharacterSelectable CurrentSelected;

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
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out CharacterSelectable newTarget))
            {
                CurrentSelected = newTarget;
                OnCharacterSelected?.Invoke(CurrentSelected);
                ConfirmBtn.interactable = true;
            }
        }
    }
}