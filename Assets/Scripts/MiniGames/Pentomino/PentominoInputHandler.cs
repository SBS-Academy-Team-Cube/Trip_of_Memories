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

    private void Awake() 
    { 
        // _tileSize *= transform.lossyScale;
        _inputActions = new PentominoInputAction();
    }
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
        if (_currentPicked != null)
        {
            FollowMouse();// Move piece with mouse

            HandleRotation();// Handle A/D key rotation
        }
    }

    public void Init()
    {

    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if (_currentPicked == null) // pick
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _pieceLayer))
            {
                IPentominoPickable pick = hit.collider.GetComponentInParent<IPentominoPickable>();
                _currentPiece = hit.collider.GetComponentInParent<PentominoPiece>();
                if (pick != null && _currentPiece != null)
                {
                    _currentPicked = pick;
                    pick.PickUp(board.GridSize);

                    BoardPos[] currentBoardPos = GetBoardPos(_currentPiece.PieceShape, _currentPiece.Transform.position);
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
                if (board.IsPlace(tryPositions))// Check if placement is valid
                {
                    _currentPicked.Place(snappedPos);//

                    board.SetActiveBoard(tryPositions, true);// Mark board positions as occupied

                    _currentPicked = null;
                    _currentPiece = null;

                    board.IsGameClearCheck();// Game Clear Check;
                }
                else // 이미 조각이 놓여져있다면
                {
                    _currentPiece.ReturnToStart();// Invalid position → return to start
                    _currentPicked = null;
                    _currentPiece = null;
                }
            }
            else // 보드 밖에 놓았다면
            {
                _currentPiece.ReturnToStart();
                _currentPicked = null;
                _currentPiece = null;
            }
        }
    }

    private void FollowMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.1f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _boardLayer))
        {
            Vector3 snappedPos = SnapToGrid(hit.point);
            Vector3 previewPos = new Vector3(snappedPos.x, snappedPos.y, snappedPos.z);
            _currentPicked.Transform.position = previewPos;
        }
    }

    private void HandleRotation()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            _currentPicked.Transform.Rotate(0, -90f, 0, Space.World);
            _currentPiece.RotatePiecePos(false);// false == left
        }

        
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            _currentPicked.Transform.Rotate(0, 90f, 0, Space.World);
            _currentPiece.RotatePiecePos(true); // true == right
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

