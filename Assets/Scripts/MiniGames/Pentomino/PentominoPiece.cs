using UnityEngine;

public class PentominoPiece : MonoBehaviour
{
    [SerializeField] private PieceShape pieceShape;
    private PieceShape curShape;
    public BoardPos[] PieceShape => curShape.shape;
    private Vector3 OriginPosition;
    private Quaternion OriginRotation;
    private BoardPos[] OriginShape;



    private bool _isPlaced = false;
    public bool IsPlaced => _isPlaced;

    private bool _isPicked = false;
    public bool IsPicked => _isPicked;

    public void Init()
    {
        if (curShape == null)
        {
            curShape = Instantiate(pieceShape);
            SaveOriginState();
        }
        else
        {
            ReturnToOrigin();
        }

    }
    public void RotatePiece(bool isRight)
    {
        transform.Rotate(0, isRight ? 90f : -90f, 0, Space.World);
        for (int i = 0; i < curShape.shape.Length; ++i)
        {
            int x = curShape.shape[i].x;
            int y = curShape.shape[i].y;
            curShape.shape[i] = new BoardPos
            {
                x = isRight ? y : -y,
                y = isRight ? -x : x
            };
        }
    }
    public void PickUp(float gridSize)
    {
        _isPicked = true;
        _isPlaced = false;

        transform.position += Vector3.up * 1.5f * gridSize;
    }
    public void ReturnToOrigin()
    {
        _isPicked = false;
        _isPlaced = false;

        transform.rotation = OriginRotation;
        transform.position = OriginPosition;
        curShape.shape = CopyShape(OriginShape);
    }
    public void Place(Vector3 position)
    {
        _isPicked = false;
        _isPlaced = true;

        transform.position = new Vector3(position.x, position.y, position.z);
    }
    private void SaveOriginState()
    {
        OriginPosition = transform.position;
        OriginRotation = transform.rotation;
        OriginShape = CopyShape(curShape.shape);
    }
    private BoardPos[] CopyShape(BoardPos[] source)
    {
        BoardPos[] result = new BoardPos[source.Length];
        for (int i = 0; i < source.Length; i++)
        {
            result[i] = source[i];
        }
        return result;
    }
}
