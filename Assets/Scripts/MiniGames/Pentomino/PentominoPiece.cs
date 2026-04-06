using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceHighlighter))]
public class PentominoPiece : MonoBehaviour, IPentominoPickable, IHighlightable
{
    [SerializeField] private PieceShape pieceShape;

    public BoardPos[] PiecePos => pieceShape.shape;

    private bool _isPicked = false;
    private PieceHighlighter _highlighter;
    public bool IsPicked => _isPicked;
    public Transform Transform => transform;

    public BoardPos[] GetBoardPositions(Vector3 currentWorldPos)
    {
        BoardPos[] result = new BoardPos[pieceShape.shape.Length];
        int baseX = Mathf.RoundToInt(currentWorldPos.x);  // ½º³ÀµÈ X
        int baseZ = Mathf.RoundToInt(currentWorldPos.z);  // ½º³ÀµÈ Z

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
    
    private void Awake()
    {
        _highlighter = GetComponent<PieceHighlighter>();
    }

    public void PickUp()
    {
        _isPicked = true;
        transform.position += Vector3.up * 1.5f;
        _highlighter.HighlightOff();
    }

    public void Place(Vector3 position)
    {
        _isPicked = false;
        transform.position = new Vector3(position.x, 0.01f, position.z);

        DebugPiecePos();
    }


    public void HighlightOn() => _highlighter.HighlightOn();
    public void HighlightOff() => _highlighter.HighlightOff();

    private void DebugPiecePos()
    {
        Debug.Log($"{gameObject.name} worldPos = {transform.position}," +
            $"PiecePos = {pieceShape.shape}");
    }
}
