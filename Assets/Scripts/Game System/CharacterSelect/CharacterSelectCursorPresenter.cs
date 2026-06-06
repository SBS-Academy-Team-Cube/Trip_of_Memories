using UnityEngine;

public class CharacterSelectCursorPresenter : MonoBehaviour
{
    [SerializeField] private CharacterSelectController Controller;

    private void Awake()
    {
        if (Controller == null)
        {
            TryGetComponent(out Controller);
        }
    }

    private void OnEnable()
    {
        if (Controller != null)
        {
            Controller.OnSelectableHoverChanged += HandleSelectableHoverChanged;
        }
    }

    private void OnDisable()
    {
        if (Controller != null)
        {
            Controller.OnSelectableHoverChanged -= HandleSelectableHoverChanged;
        }

        if (MouseCursorManager.Instance != null)
        {
            MouseCursorManager.Instance.ResetVisual();
        }
    }

    private void HandleSelectableHoverChanged(CharacterSelectable Selectable)
    {
        if (MouseCursorManager.Instance == null)
        {
            return;
        }

        MouseCursorManager.Instance.SetVisual(Selectable != null ? MouseCursorVisual.Selectable : MouseCursorVisual.Default);
    }
}
