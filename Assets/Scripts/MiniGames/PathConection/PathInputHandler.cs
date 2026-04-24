using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ColorButton
{
    public PathColor pathColor;
    public Button colorButton;
}
[Serializable]
public struct ColorPiece
{
    public PathColor pathColor;
    public PathPiece piece;
}
public class PathInputHandler : MonoBehaviour
{
    [SerializeField] private ColorPiece[] pathPieces;
    [SerializeField] private ColorButton[] button;


    private void OnEnable()
    {
        foreach(var btn in button)
        {
            btn.colorButton.onClick.AddListener(() => OnClickColorButton(btn.pathColor));
        }
    }



    private void OnClickColorButton(PathColor color)
    {
        foreach(var piece in pathPieces)
        {
            if(piece.pathColor == color)
            {
                piece.piece.PieceRoll();
            }
        }
    }
}
