using UnityEngine;
using UnityEngine.InputSystem;

public class PentominoInputHandler : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _pieceLayer;
    [SerializeField] private LayerMask _boardLayer;
    [SerializeField] private float _tileSize = 1f;
    [SerializeField] private float _targetBoundsMultiplier = 0.85f;

    private PentominoInputAction _inputActions;
    private IPentominoPickable _currentPicked = null;
    private IHighlightable _currentHover = null;

    private void Awake() { _inputActions = new PentominoInputAction(); }
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Gameplay.Click.performed += OnClickPerformed;
    }
    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Gameplay.Click.performed -= OnClickPerformed;
    }

    private void Update()
    {
        HandleHover();
        if (_currentPicked != null) FollowMouse();
    }

    private void HandleHover()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _pieceLayer))
        {
            IHighlightable hl = hit.collider.GetComponentInParent<IHighlightable>();
            if (hl != null && hl != _currentHover)
            {
                _currentHover?.HighlightOff();
                _currentHover = hl;
                _currentHover.HighlightOn();
            }
        }
        else
        {
            _currentHover?.HighlightOff();
            _currentHover = null;
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if (_currentPicked == null)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _pieceLayer))
            {
                IPentominoPickable pick = hit.collider.GetComponentInParent<IPentominoPickable>();
                if (pick != null)
                {
                    _currentPicked = pick;
                    pick.PickUp();
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _boardLayer))
            {
                Vector3 snappedPos = SnapToGrid(hit.point);

                if (!IsPositionOccupied(snappedPos))
                {
                    _currentPicked.Place(snappedPos);
                    _currentPicked = null;
                }
                else
                {
                    Debug.LogWarning("rrr");
                }
            }
        }
    }

    private void FollowMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _boardLayer))
        {
            Vector3 snappedPos = SnapToGrid(hit.point);
            Vector3 previewPos = new Vector3(snappedPos.x, 0.6f, snappedPos.z);
            _currentPicked.Transform.position = previewPos;
        }
    }

    private Vector3 SnapToGrid(Vector3 worldPos)
    {
        float x = Mathf.Round(worldPos.x / _tileSize) * _tileSize;
        float z = Mathf.Round(worldPos.z / _tileSize) * _tileSize;
        return new Vector3(x, 0.01f, z);
    }

    private bool IsPositionOccupied(Vector3 targetPos)
    {
        PentominoPiece[] allPieces = FindObjectsOfType<PentominoPiece>();

        // 놓으려는 위치를 더 여유 있게 만듦
        Bounds targetBounds = new Bounds(targetPos,
            new Vector3(_tileSize * _targetBoundsMultiplier,
                        0.15f,
                        _tileSize * _targetBoundsMultiplier));

        foreach (var other in allPieces)
        {
            if (other.IsPicked) continue;

            Bounds otherBounds = other.GetBounds();

            bool intersects = otherBounds.Intersects(targetBounds);

            Debug.Log($"[Bounds Check] {other.gameObject.name} → Intersects = {intersects} (Multiplier: {_targetBoundsMultiplier})");

            if (intersects)
                return true;
        }
        return false;
    }
}

