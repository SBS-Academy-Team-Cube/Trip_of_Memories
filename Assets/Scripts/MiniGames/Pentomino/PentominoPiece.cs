using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceHighlighter))]
public class PentominoPiece : MonoBehaviour, IPentominoPickable, IHighlightable
{
    [SerializeField] private BoardPos[] piecePos; // Shape of the piece

    public BoardPos[] PiecePos => piecePos;

    private bool _isPicked = false;
    private PieceHighlighter _highlighter;
    public bool IsPicked => _isPicked;
    public Transform Transform => transform;

    public void RotatePiecePos(bool isRight)
    {
        //if isright == true -> 90
        //if isright == false -> -90
        if(isRight)
        {
            for (int i = 0; i < piecePos.Length; ++i)
            {
                int x = piecePos[i].x;
                int y = piecePos[i].y;
                BoardPos newPiece = new BoardPos();
                newPiece.x = y;
                newPiece.y = -x;
                piecePos[i] = newPiece;
            }
        }
        else
        {
            for (int i = 0; i < piecePos.Length; ++i)
            {
                int x = piecePos[i].x;
                int y = piecePos[i].y;
                BoardPos newPiece = new BoardPos();
                newPiece.x = -y;
                newPiece.y = x;
                piecePos[i] = newPiece;
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
    }

    public void HighlightOn() => _highlighter.HighlightOn();
    public void HighlightOff() => _highlighter.HighlightOff();


}
