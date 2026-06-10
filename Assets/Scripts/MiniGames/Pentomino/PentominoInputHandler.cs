using UnityEngine;
using UnityEngine.InputSystem;

public class PentominoInputHandler : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _pieceLayer;
    [SerializeField] private LayerMask _boardLayer;

    [SerializeField] private PentominoBoard board;

    private PentominoInputAction _inputActions;
    private IPentominoPickable _currentPicked = null;
    private PentominoPiece _currentPiece = null;
    private BoardPos[] _previousBoardPos = null;
    private bool _shouldRestorePreviousBoard = false;

    private void Awake()
    {
        _inputActions = new PentominoInputAction();
    }
    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Gameplay.Click.performed -= OnClickPerformed;
    }

    private void Update()
    {
        if (_currentPicked != null)
        {
            FollowMouse();
            HandleRotation();
        }
    }

    public void Init()
    {
        _inputActions.Enable();
        _inputActions.Gameplay.Click.performed += OnClickPerformed;
    }
    public void Clear()
    {
        _inputActions.Disable();
        _inputActions.Gameplay.Click.performed -= OnClickPerformed;
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
                _currentPiece = hit.collider.GetComponentInParent<PentominoPiece>();
                if (pick != null && _currentPiece != null)
                {
                    _currentPicked = pick;
                    BoardPos[] currentBoardPos = GetBoardPos(_currentPiece.PieceShape, _currentPiece.Transform.position);
                    _previousBoardPos = currentBoardPos;
                    _shouldRestorePreviousBoard = board.IsActiveBoard(currentBoardPos);

                    pick.PickUp(board.GridSize);

                    if (_shouldRestorePreviousBoard)
                        board.SetActiveBoard(currentBoardPos, false);// Clear old position on board
                }
            }
        }
        else // place
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _boardLayer))
            {
                Vector3 snappedPos = SnapToGrid(hit.point);
                BoardPos[] tryPositions = GetBoardPos(_currentPiece.PieceShape, snappedPos);
                if (board.CanPlace(tryPositions))// Check if placement is valid
                {
                    _currentPicked.Place(snappedPos);
                    board.SetActiveBoard(tryPositions, true);// Mark board positions as occupied
                    ClearCurrentPiece();
                    board.IsGameClearCheck();// Game Clear Check;
                }
                else
                {
                    Debug.Log("Other Piece already taken");
                    ReturnCurrentPiece();
                }
            }
            else // 보드 밖에 놓았다면
            {
                Debug.Log("There's invalid place");
                RemoveCurrentPieceFromBoard();
            }
        }
    }
    private void RemoveCurrentPieceFromBoard()
    {
        _currentPiece.ReturnToHome();
        ClearCurrentPiece();
    }

    private void ReturnCurrentPiece()
    {
        _currentPiece.ReturnToStart();
        if (_shouldRestorePreviousBoard && _previousBoardPos != null)
            board.SetActiveBoard(_previousBoardPos, true);

        ClearCurrentPiece();
    }

    private void ClearCurrentPiece()
    {
        _currentPicked = null;
        _currentPiece = null;
        _previousBoardPos = null;
        _shouldRestorePreviousBoard = false;
    }
    private void FollowMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _boardLayer))
        {
            Vector3 snappedPos = SnapToGrid(hit.point);
            _currentPicked.Transform.position = snappedPos;
        }
    }

    private void HandleRotation()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            _currentPicked.Transform.Rotate(0, -90f, 0, Space.World);
            _currentPiece.RotatePiecePos(false);
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            _currentPicked.Transform.Rotate(0, 90f, 0, Space.World);
            _currentPiece.RotatePiecePos(true);
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

