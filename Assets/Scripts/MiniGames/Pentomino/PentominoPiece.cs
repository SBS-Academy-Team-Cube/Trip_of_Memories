using UnityEngine;

public class PentominoPiece : MonoBehaviour, IPentominoPickable
{
    [SerializeField] private PieceShape pieceShape;
    private Vector2 TransformPivotOffset;
    private Vector3 homePos;
    private Quaternion homeRot;
    private BoardPos[] homeShape;
    private Vector3 startPos;
    private Quaternion startRot;
    private BoardPos[] startShape;
    private PieceShape curShape;

    public BoardPos[] PieceShape => curShape.shape;

    private bool _isPlaced = false;
    public bool IsPlaced => _isPlaced;

    private bool _isPicked = false;
    public bool IsPicked => _isPicked;
    public Transform Transform => transform;

    public void Init()
    {
        curShape = Instantiate(pieceShape);
        // InitRotateSet();
        SaveHomeState();
        SavePrevState();
    }
    private void InitRotateSet()
    {
        float rotationY = transform.eulerAngles.y;

        int rotate = Mathf.RoundToInt(rotationY / 90f) % 4;
        if (rotate < 0)
            rotate += 4;

        for (int i = 0; i < rotate; i++)
            RotatePiecePos(true);
    }

    public void RotatePiecePos(bool isRight)
    {
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
        // if (isRight)
        // {
        //     for (int i = 0; i < curShape.shape.Length; ++i)
        //     {
        //         int x = curShape.shape[i].x;
        //         int y = curShape.shape[i].y;
        //         // BoardPos newPiece =
        //         curShape.shape[i] = new BoardPos
        //         {
        //             x = isRight ? y : y,
        //             y = isRight ? -x : x
        //         };
        //     }
        // }
        // else
        // {
        //     for (int i = 0; i < curShape.shape.Length; ++i)
        //     {
        //         int x = curShape.shape[i].x;
        //         int y = curShape.shape[i].y;
        //         BoardPos newPiece = new BoardPos();
        //         newPiece.x = -y;
        //         newPiece.y = x;
        //         curShape.shape[i] = newPiece;
        //     }
        // }
    }
    public void PickUp(float gridSize)
    {
        _isPicked = true;
        transform.position += Vector3.up * 1.5f * gridSize;
    }
    public void ReturnToStart()
    {
        _isPicked = false;
        transform.rotation = startRot;
        transform.position = startPos;
        curShape.shape = CopyShape(startShape);

    }
    public void ReturnToHome()
    {
        _isPicked = false;
        transform.rotation = homeRot;
        transform.position = homePos;
        curShape.shape = CopyShape(homeShape);
        SavePrevState();
    }
    public void Place(Vector3 position)
    {
        _isPicked = false;
        _isPlaced = true;

        transform.position = new Vector3(position.x, position.y, position.z);
        SavePrevState();

        DebugPiecePos();
    }

    private void SavePrevState()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        startShape = CopyShape(curShape.shape);
    }

    private void SaveHomeState()
    {
        homePos = transform.position;
        homeRot = transform.rotation;
        homeShape = CopyShape(curShape.shape);
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
    private void DebugPiecePos()
    {
        Debug.Log($"{gameObject.name} worldPos = {transform.position}," +
            $"PiecePos = {curShape.shape}");
    }
}
