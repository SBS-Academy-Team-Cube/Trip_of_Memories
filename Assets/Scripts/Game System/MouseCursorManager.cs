using UnityEngine;

public enum MouseCursorVisual
{
    Default,
    Selectable
}

public class MouseCursorManager : Singleton<MouseCursorManager>
{
    [Header("Cursor Textures")]
    [SerializeField] private Texture2D DefaultCursor;
    [SerializeField] private Texture2D SelectableCursor;
    
    [Header("Hotspots")]
    [SerializeField] private Vector2 DefaultHotspot = Vector2.zero;
    [SerializeField] private Vector2 SelectableHotspot = Vector2.zero;

    [SerializeField] private CursorMode Mode = CursorMode.Auto;

    private MouseCursorVisual CurrentVisual = MouseCursorVisual.Default;
    private bool bVisible = true;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            return;
        }

        ApplyVisibility();
        ApplyVisual();
    }

    public void SetVisible(bool bShow)
    {
        if (bVisible == bShow)
        {
            return;
        }

        bVisible = bShow;
        ApplyVisibility();

        if (bVisible)
        {
            ApplyVisual();
        }
    }

    public void SetVisual(MouseCursorVisual Visual)
    {
        if (CurrentVisual == Visual)
        {
            return;
        }

        CurrentVisual = Visual;

        if (bVisible)
        {
            ApplyVisual();
        }
    }

    public void ResetVisual()
    {
        SetVisual(MouseCursorVisual.Default);
    }

    private void ApplyVisibility()
    {
        Cursor.lockState = bVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = bVisible;
    }

    private void ApplyVisual()
    {
        Texture2D texture = CurrentVisual == MouseCursorVisual.Selectable ? SelectableCursor : DefaultCursor;
        Vector2 hotspot = CurrentVisual == MouseCursorVisual.Selectable ? SelectableHotspot : DefaultHotspot;
        Cursor.SetCursor(texture, hotspot, Mode);
    }
}
