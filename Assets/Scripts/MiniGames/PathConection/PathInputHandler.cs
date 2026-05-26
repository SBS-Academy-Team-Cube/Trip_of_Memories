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
    [SerializeField] private PathPuzzleUIMng UIMng;
    [SerializeField] private ColorButton[] button;


    public void GameStart()
    {

    }
    private void OnEnable()
    {
        foreach(var btn in button)
        {
            btn.colorButton.onClick.AddListener(() => OnClickColorButton(btn.pathColor));
        }
    }



    private void OnClickColorButton(PathColor color)
    {
        UIMng.Roll(color);
    }



}
