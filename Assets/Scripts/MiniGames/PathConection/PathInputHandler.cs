using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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

    private int greenRollCount = 0;
    private int redRollCount = 0;
    private int blueRollCount = 0;

    private bool isRolling = false;

    private Coroutine coroutine;

    public int GreenRollCount => greenRollCount;
    public int RedRollCount => redRollCount;
    public int BlueRollCount => blueRollCount;

    public UnityEvent ClearCheckEvent = new();



    private void OnEnable()
    {
        foreach(var btn in button)
        {
            btn.colorButton.onClick.AddListener(() => OnClickColorButton(btn.pathColor));
        }
    }



    private void OnClickColorButton(PathColor color)
    {
        if(isRolling)
           return;

        coroutine = StartCoroutine(RollCoroutine(color));
    }
    private IEnumerator RollCoroutine(PathColor color)
    {
        isRolling = true;
        RollCountPlus(color);
        foreach (var piece in pathPieces)
        {
            if (piece.pathColor == color)
            {
                piece.piece.PieceRoll();
            }
        }
        yield return new WaitForSeconds(PathPiece.RotateDuration);

        ClearCheckEvent?.Invoke();
        isRolling = false;
    }

    private void RollCountPlus(PathColor color)
    {
        switch(color)
        {
            case PathColor.Green:
                greenRollCount++;
                break;
            case PathColor.Blue:
                blueRollCount++;
                break;
                case PathColor.Red:
                redRollCount++;
                break;
            default:
                break;
        }
        DebugRollCount();
    }
    private void DebugRollCount()
    {
        Debug.Log($"Green : {greenRollCount % 4}, Blue : {blueRollCount % 4}, Red : {redRollCount % 4}");
    }

}
