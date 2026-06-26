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
        Texture2D texture = CurrentVisual == MouseCursorVisual.Selectable ? SelectableCursor : DefaultCursor;
        Vector2 hotspot = CurrentVisual == MouseCursorVisual.Selectable ? SelectableHotspot : DefaultHotspot;
        Cursor.SetCursor(texture, hotspot, Mode);
    }
}
