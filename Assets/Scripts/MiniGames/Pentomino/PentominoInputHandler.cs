using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PentominoInputHandler : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _pieceLayer;
    [SerializeField] private LayerMask _boardLayer;
    [SerializeField] private PentominoBoard board;
    public event Action OnQuitGame;
    private PentominoInputAction _inputActions;
    private PentominoPiece _currentPiece = null;
    private void Awake()
    {
        _inputActions = new PentominoInputAction();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Gameplay.Click.performed -= OnClickPerformed;
        _inputActions.Gameplay.Escape.performed -= OnEscape;
        board.OnClear -= Clear;
    }
    private void Update()
    {
        if (_currentPiece != null)
        {
            FollowMouse();
            HandleRotation();
        }
    }
    public void Init()
    {
        _inputActions.Enable();
        _inputActions.Gameplay.Click.performed += OnClickPerformed;
        _inputActions.Gameplay.Escape.performed += OnEscape;
        board.OnClear += Clear;
    }
    public void Clear()
    {
        _inputActions.Disable();
        _inputActions.Gameplay.Click.performed -= OnClickPerformed;
        _inputActions.Gameplay.Escape.performed -= OnEscape;
        board.OnClear -= Clear;
    }
    private void OnEscape(InputAction.CallbackContext context)
    {
        OnQuitGame?.Invoke();
    }
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);
        if (_currentPiece == null)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _pieceLayer))
            {
                _currentPiece = hit.collider.GetComponentInParent<PentominoPiece>();
                if (_currentPiece != null)
                {
                    bool IsPlacedPiece = _currentPiece.IsPlaced;
                    _currentPiece.PickUp(board.GridSize);
                    if (IsPlacedPiece)
                        board.SetActiveBoard(GetBoardPos(_currentPiece.PieceShape, _currentPiece.transform.position), false);
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _boardLayer))
            {
                Vector3 snappedPos = SnapToGrid(hit.point);
                BoardPos[] tryPositions = GetBoardPos(_currentPiece.PieceShape, snappedPos);
                if (board.CanPlace(tryPositions))   // Check if placement is valid
                {
                    _currentPiece.Place(snappedPos);
                    board.SetActiveBoard(tryPositions, true);   // Mark board positions as occupied
                    ClearCurrentPiece();
                    return;
                }
            }
            RemoveCurrentPieceFromBoard();
        }
    }
    private void RemoveCurrentPieceFromBoard()
    {
        if (_currentPiece)
        {
            _currentPiece.ReturnToOrigin();
            ClearCurrentPiece();
        }
    }
    private void ClearCurrentPiece()
    {
        _currentPiece = null;
    }
    private void FollowMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _boardLayer))
        {
            Vector3 snappedPos = SnapToGrid(hit.point);
            _currentPiece.transform.position = snappedPos;
        }
    }

    private void HandleRotation()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            _currentPiece.RotatePiece(false);
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            _currentPiece.RotatePiece(true);
        }
    }
    private Vector3 SnapToGrid(Vector3 worldPos)
    {
        Vector3 local = worldPos - board.ZeroPos;
        float x = Mathf.Round(local.x / board.GridSize) * board.GridSize;
        float z = Mathf.Round(local.z / board.GridSize) * board.GridSize;
        return board.ZeroPos + new Vector3(x, 0f, z);
    }
    private BoardPos[] GetBoardPos(BoardPos[] pieceShape, Vector3 worldPos)
    {
        BoardPos[] result = new BoardPos[pieceShape.Length];
        int baseX = board.WorldPosToBoardPos(worldPos).x;  // snap Pos X
        int baseZ = board.WorldPosToBoardPos(worldPos).y;  // snap Pos Z

        for (int i = 0; i < pieceShape.Length; i++)
        {
            result[i].x = baseX + pieceShape[i].x;
            result[i].y = baseZ + pieceShape[i].y;
        }
        return result;
    }
}

