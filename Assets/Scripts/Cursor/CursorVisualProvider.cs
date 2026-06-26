using UnityEngine;

public class CursorVisualProvider : MonoBehaviour, ICursorVisualProvider
{
    [SerializeField] private MouseCursorVisual Visual = MouseCursorVisual.Selectable;

    public MouseCursorVisual GetCursorVisual()
    {
        return Visual;
    }
}
