using UnityEngine;

public class PentominoPiece : MonoBehaviour, IPentominoPickable
{
    [SerializeField] private PieceShape pieceShape;

    private Vector3 startPos;
    private Quaternion startRot;

    public BoardPos[] PiecePos => pieceShape.shape;

    private bool _isPicked = false;
    public bool IsPicked => _isPicked;
    public Transform Transform => transform;

    public BoardPos[] GetBoardPositions(Vector3 currentWorldPos)
    {
        BoardPos[] result = new BoardPos[pieceShape.shape.Length];
        int baseX = Mathf.RoundToInt(currentWorldPos.x);  // snap Pos X
        int baseZ = Mathf.RoundToInt(currentWorldPos.z);  // snap Pos Z

        for (int i = 0; i < pieceShape.shape.Length; i++)
        {
            result[i].x = baseX + pieceShape.shape[i].x;
            result[i].y = baseZ + pieceShape.shape[i].y;
        }
        return result;
    }

    public void RotatePiecePos(bool isRight)
    {
        //if isright == true -> 90
        //if isright == false -> -90
        if(isRight)
        {
            for (int i = 0; i < pieceShape.shape.Length; ++i)
            {
                int x = pieceShape.shape[i].x;
                int y = pieceShape.shape[i].y;
                BoardPos newPiece = new BoardPos();
                newPiece.x = y;
                newPiece.y = -x;
                pieceShape.shape[i] = newPiece;
            }
        }
        else
        {
            for (int i = 0; i < pieceShape.shape.Length; ++i)
            {
                int x = pieceShape.shape[i].x;
                int y = pieceShape.shape[i].y;
                BoardPos newPiece = new BoardPos();
                newPiece.x = -y;
                newPiece.y = x;
                pieceShape.shape[i] = newPiece;
            }
        }
    }
    
    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void PickUp()
    {
        _isPicked = true;
        transform.position += Vector3.up * 1.5f;
    }
    public void ReturnToStart()
    {
        _isPicked = false;
        transform.rotation = startRot;
        transform.position = startPos;
        
    }

    public void Place(Vector3 position)
    {
        _isPicked = false;
        transform.position = new Vector3(position.x, 0.01f, position.z);

        DebugPiecePos();
    }

    private void DebugPiecePos()
    {
        Debug.Log($"{gameObject.name} worldPos = {transform.position}," +
            $"PiecePos = {pieceShape.shape}");
    }
}
