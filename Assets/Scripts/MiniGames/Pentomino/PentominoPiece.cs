using UnityEngine;

public class PentominoPiece : MonoBehaviour, IPentominoPickable
{
    [SerializeField] private PieceShape pieceShape;
    [SerializeField] private Transform PivotTransform;

    private Vector2 TransformPivotOffset;

    private Vector3 startPos;
    private Quaternion startRot;
    private PieceShape curShape;

    public BoardPos[] PieceShape => curShape.shape;

    private bool _isPicked = false;
    public bool IsPicked => _isPicked;
    public Transform Transform => transform;

    public void Init()
    {
        startPos = PivotTransform.position;
        startRot = PivotTransform.rotation;
        curShape = Instantiate(pieceShape);

        Vector3 Delta = transform.position - PivotTransform.position;
        TransformPivotOffset = new Vector2(Delta.x, Delta.z);

        transform.position = startPos;
        transform.rotation = startRot;
        InitRotateSet();
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
        //if isright == true -> 90
        //if isright == false -> -90
        if (isRight)
        {
            for (int i = 0; i < curShape.shape.Length; ++i)
            {
                int x = curShape.shape[i].x;
                int y = curShape.shape[i].y;
                BoardPos newPiece = new BoardPos();
                newPiece.x = y;
                newPiece.y = -x;
                curShape.shape[i] = newPiece;
            }
        }
        else
        {
            for (int i = 0; i < curShape.shape.Length; ++i)
            {
                int x = curShape.shape[i].x;
                int y = curShape.shape[i].y;
                BoardPos newPiece = new BoardPos();
                newPiece.x = -y;
                newPiece.y = x;
                curShape.shape[i] = newPiece;
            }
        }
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

    }
    public void SetPreviewPosition(Vector3 Target)
    {
        // transform.position
        transform.position = new Vector3(Target.x + TransformPivotOffset.x, Target.y, Target.z + TransformPivotOffset.y);

    }

    public void Place(Vector3 position)
    {
        _isPicked = false;
        transform.position = new Vector3(position.x, position.y, position.z);

        DebugPiecePos();
    }

    private void DebugPiecePos()
    {
        Debug.Log($"{gameObject.name} worldPos = {transform.position}," +
            $"PiecePos = {curShape.shape}");
    }
}
