using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class PathPuzzleUIMng : MonoBehaviour
{
    [SerializeField] private GameObject GameCanvas;

    [SerializeField] private ColorPiece[] pathPieces;

    [SerializeField] private RectTransform[] pieceObj;

    private Quaternion[] startRot;

    private bool isRolling = false;

    private Coroutine coroutine;

    public UnityEvent ClearEvent = new();

    private int greenRollCount = 0;
    private int redRollCount = 0;
    private int blueRollCount = 0;

    public int GreenRollCount => greenRollCount;
    public int RedRollCount => redRollCount;
    public int BlueRollCount => blueRollCount;

    private void Awake()
    {

    }



    public void GameStart()
    {
        if(startRot == null)
        {
            for (int i = 0; i < pieceObj.Length; i++)
            {
                startRot[i] = pieceObj[i].localRotation;
            }
        }
        GameCanvas.SetActive(true);
        GameReset();
    }
    public void GameStop()
    {
        GameCanvas.SetActive(false);
    }

    public void GameReset()
    {
        for(int i = 0; i < pieceObj.Length; i++)
        {
            pieceObj[i].rotation = startRot[i];
        }
    }
    public void Roll(PathColor color)
    {
        if (isRolling)
            return;
        RollCountPlus(color);
        coroutine = StartCoroutine(RollCoroutine(color));
    }

    private IEnumerator RollCoroutine(PathColor color)
    {
        isRolling = true;

        foreach (var piece in pathPieces)
        {
            if (piece.pathColor == color)
            {
                piece.piece.PieceRoll();
            }
        }
        yield return new WaitForSeconds(PathPiece.RotateDuration);

        ClearEvent?.Invoke();
        isRolling = false;
    }


    private void RollCountPlus(PathColor color)
    {
        switch (color)
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




