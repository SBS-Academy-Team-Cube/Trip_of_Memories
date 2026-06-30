using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum MouseCursorVisual
{
    Default,
    Selectable
}

public class MouseCursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    [SerializeField] private Texture2D DefaultCursor;
    [SerializeField] private Texture2D SelectableCursor;

    [Header("Hotspots")]
    [SerializeField] private Vector2 DefaultHotspot = Vector2.zero;
    [SerializeField] private Vector2 SelectableHotspot = Vector2.zero;

    [Header("Cursor Size")]
    [SerializeField, Min(1)] private int CursorSize = 32;
    [SerializeField] private bool bScaleWithDisplayDpi = true;
    [SerializeField, Min(1f)] private float ReferenceDpi = 96f;
    [SerializeField, Min(1f)] private float MaxDpiScale = 2f;

    [SerializeField] private CursorMode Mode = CursorMode.Auto;

    [Header("Pointer Raycast")]
    [SerializeField] private Camera RaycastCamera;
    [SerializeField] private LayerMask WorldRaycastLayerMask = ~0;
    [SerializeField] private float WorldRaycastDistance = 100f;
    [SerializeField] private QueryTriggerInteraction WorldTriggerInteraction = QueryTriggerInteraction.Ignore;
    [SerializeField, Min(1)] private int MaxWorldRaycastHits = 32;

    private MouseCursorVisual CurrentVisual = MouseCursorVisual.Default;
    private bool bVisible = true;
    private EventSystem PointerEventSystem;
    private PointerEventData PointerEventData;
    private readonly List<RaycastResult> UIRaycastResults = new List<RaycastResult>();
    private RaycastHit[] WorldRaycastHits;
    private Texture2D ScaledDefaultCursor;
    private Texture2D ScaledSelectableCursor;
    private int CachedCursorPixelSize = -1;
    private int AppliedCursorPixelSize = -1;

    void Awake()
    {
        ApplyVisibility();
        ApplyVisual();
    }
    void Update()
    {
        if (!bVisible)
        {
            return;
        }
        SetVisual(GetPointerVisual());
        RefreshCursorScaleIfNeeded();
    }
    private MouseCursorVisual GetPointerVisual()
    {
        if (TryGetCursorVisualFromUI(out var uiVisual))
        {
            return uiVisual;
        }
        if (TryGetCursorVisualFromWorld(out var worldVisual))
        {
            return worldVisual;
        }
        return MouseCursorVisual.Default;
    }
    bool TryGetCursorVisualFromUI(out MouseCursorVisual outVisual)
    {
        outVisual = MouseCursorVisual.Default;
        if (EventSystem.current == null || !TryGetMousePosition(out Vector2 mousePosition))
        {
            return false;
        }

        if (PointerEventData == null || PointerEventSystem != EventSystem.current)
        {
            PointerEventSystem = EventSystem.current;
            PointerEventData = new PointerEventData(PointerEventSystem);
        }

        PointerEventData.Reset();
        PointerEventData.position = mousePosition;

        UIRaycastResults.Clear();
        EventSystem.current.RaycastAll(PointerEventData, UIRaycastResults);

        for (int i = 0; i < UIRaycastResults.Count; i++)
        {
            if (TryGetCursorVisual(UIRaycastResults[i].gameObject, out outVisual))
            {
                return true;
            }
        }

        return false;
    }
    bool TryGetCursorVisualFromWorld(out MouseCursorVisual outVisual)
    {
        outVisual = MouseCursorVisual.Default;

        Camera targetCamera = RaycastCamera != null ? RaycastCamera : Camera.main;
        if (targetCamera == null)
        {
            return false;
        }

        if (!TryGetMousePosition(out Vector2 mousePosition))
        {
            return false;
        }

        Ray ray = targetCamera.ScreenPointToRay(mousePosition);
        if (WorldRaycastHits == null || WorldRaycastHits.Length != MaxWorldRaycastHits)
        {
            WorldRaycastHits = new RaycastHit[MaxWorldRaycastHits];
        }

        int hitCount = Physics.RaycastNonAlloc(ray, WorldRaycastHits, WorldRaycastDistance, WorldRaycastLayerMask, WorldTriggerInteraction);
        if (hitCount == 0)
        {
            return false;
        }

        bool bFoundVisual = false;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit hit = WorldRaycastHits[i];
            if (hit.distance >= closestDistance)
            {
                continue;
            }

            if (hit.collider == null || !TryGetCursorVisual(hit.collider.gameObject, out MouseCursorVisual hitVisual))
            {
                continue;
            }

            closestDistance = hit.distance;
            outVisual = hitVisual;
            bFoundVisual = true;
        }

        return bFoundVisual;
    }

    private bool TryGetMousePosition(out Vector2 outPosition)
    {
        outPosition = Vector2.zero;
        if (Mouse.current == null)
        {
            return false;
        }

        outPosition = Mouse.current.position.ReadValue();
        return true;
    }

    private bool TryGetCursorVisual(GameObject Target, out MouseCursorVisual outVisual)
    {
        outVisual = MouseCursorVisual.Default;
        if (Target == null)
        {
            return false;
        }

        ICursorVisualProvider Provider = Target.GetComponentInParent<ICursorVisualProvider>();
        if (Provider == null)
        {
            return false;
        }

        if (Provider is MonoBehaviour Behaviour && !Behaviour.isActiveAndEnabled)
        {
            return false;
        }

        outVisual = Provider.GetCursorVisual();
        return true;
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
        Texture2D sourceTexture = CurrentVisual == MouseCursorVisual.Selectable ? SelectableCursor : DefaultCursor;
        Vector2 sourceHotspot = CurrentVisual == MouseCursorVisual.Selectable ? SelectableHotspot : DefaultHotspot;
        Texture2D texture = GetCursorTexture(CurrentVisual, sourceTexture);
        Vector2 hotspot = GetScaledHotspot(sourceTexture, texture, sourceHotspot);
        AppliedCursorPixelSize = GetCursorPixelSize();
        Cursor.SetCursor(texture, hotspot, Mode);
    }

    private void RefreshCursorScaleIfNeeded()
    {
        if (AppliedCursorPixelSize != GetCursorPixelSize())
        {
            ApplyVisual();
        }
    }

    private Texture2D GetCursorTexture(MouseCursorVisual visual, Texture2D sourceTexture)
    {
        if (sourceTexture == null)
        {
            return null;
        }

        int cursorPixelSize = GetCursorPixelSize();
        if (CachedCursorPixelSize != cursorPixelSize)
        {
            ClearScaledCursors();
            CachedCursorPixelSize = cursorPixelSize;
        }

        if (Mathf.Max(sourceTexture.width, sourceTexture.height) == cursorPixelSize)
        {
            return sourceTexture;
        }

        if (visual == MouseCursorVisual.Selectable)
        {
            if (ScaledSelectableCursor == null)
            {
                ScaledSelectableCursor = CreateScaledCursorTexture(sourceTexture, cursorPixelSize);
            }
            return ScaledSelectableCursor;
        }

        if (ScaledDefaultCursor == null)
        {
            ScaledDefaultCursor = CreateScaledCursorTexture(sourceTexture, cursorPixelSize);
        }
        return ScaledDefaultCursor;
    }

    private int GetCursorPixelSize()
    {
        float dpiScale = 1f;
        if (bScaleWithDisplayDpi && Screen.dpi > 0f)
        {
            dpiScale = Mathf.Clamp(Screen.dpi / ReferenceDpi, 1f, MaxDpiScale);
        }

        return Mathf.Max(1, Mathf.RoundToInt(CursorSize * dpiScale));
    }

    private Vector2 GetScaledHotspot(Texture2D sourceTexture, Texture2D cursorTexture, Vector2 sourceHotspot)
    {
        if (sourceTexture == null || cursorTexture == null || sourceTexture == cursorTexture)
        {
            return sourceHotspot;
        }

        return new Vector2(
            sourceHotspot.x * cursorTexture.width / sourceTexture.width,
            sourceHotspot.y * cursorTexture.height / sourceTexture.height);
    }

    private Texture2D CreateScaledCursorTexture(Texture2D sourceTexture, int maxSize)
    {
        float scale = Mathf.Min((float)maxSize / sourceTexture.width, (float)maxSize / sourceTexture.height);
        int width = Mathf.Max(1, Mathf.RoundToInt(sourceTexture.width * scale));
        int height = Mathf.Max(1, Mathf.RoundToInt(sourceTexture.height * scale));

        RenderTexture previousRenderTexture = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        renderTexture.filterMode = FilterMode.Bilinear;

        Texture2D scaledTexture = new Texture2D(width, height, TextureFormat.RGBA32, false)
        {
            name = $"{sourceTexture.name}_cursor_{width}x{height}",
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        try
        {
            Graphics.Blit(sourceTexture, renderTexture);
            RenderTexture.active = renderTexture;
            scaledTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            scaledTexture.Apply(false, false);
        }
        finally
        {
            RenderTexture.active = previousRenderTexture;
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        return scaledTexture;
    }

    private void ClearScaledCursors()
    {
        DestroyScaledCursor(ref ScaledDefaultCursor);
        DestroyScaledCursor(ref ScaledSelectableCursor);
    }

    private void DestroyScaledCursor(ref Texture2D texture)
    {
        if (texture == null)
        {
            return;
        }

        Destroy(texture);
        texture = null;
    }

    private void OnDestroy()
    {
        ClearScaledCursors();
    }
}
