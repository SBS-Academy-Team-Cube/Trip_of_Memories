using UnityEngine;
using UnityEngine.EventSystems;

public class UICursorSelectablePresenter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData EventData)
    {
        if (MouseCursorManager.Instance != null)
        {
            MouseCursorManager.Instance.SetVisual(MouseCursorVisual.Selectable);
        }
    }

    public void OnPointerExit(PointerEventData EventData)
    {
        if (MouseCursorManager.Instance != null)
        {
            MouseCursorManager.Instance.ResetVisual();
        }
    }

    private void OnDisable()
    {
        if (MouseCursorManager.Instance != null)
        {
            MouseCursorManager.Instance.ResetVisual();
        }
    }
}
