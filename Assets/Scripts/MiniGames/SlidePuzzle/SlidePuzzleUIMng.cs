using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlidePuzzleUIMng : MonoBehaviour
{
    [Header("piecePosition")]
    [SerializeField] private RectTransform[] GridPos;


    [Header("pieces")]
    [SerializeField] private Image[] pieces;
    [SerializeField] private RectTransform[] InitPos; // 무조건 조각 순서대로

    [Header("minigameCanvas")]
    [SerializeField] private GameObject canvas;

    [Header("last Piece")]
    [SerializeField] private Image piece_9;

    [Header("Clear coroutine time")]
    [SerializeField] private float clearTime = 0f;

    // 실제 UI상에서 조각 이동하는 역할..
    private Vector2[] PieceInitPos;
    private Vector2[] pos;

    private Image curPiece;

    public void GameInit()
    {
        if (GridPos == null)
            return;

        if(PieceInitPos == null)
        {
            PieceInitPos = new Vector2[InitPos.Length];
            for (int i = 0; i < InitPos.Length; i++)
            {
                PieceInitPos[i] = new Vector2(InitPos[i].anchoredPosition.x, InitPos[i].anchoredPosition.y);
            }
        }

        if(pos == null)
        {
            pos = new Vector2[GridPos.Length];
            for (int i = 0; i < GridPos.Length; i++)
            {
                pos[i] = new Vector2(GridPos[i].anchoredPosition.x, GridPos[i].anchoredPosition.y);
            }
        }


        PieceInit();
        canvas.SetActive(true);
    }
    public void GameClear()
    {
        StartCoroutine(ClearCoroutine());
    }
    public void MovePiece(int pieceNum, int destIndex)// pieceNum = 1~8 , destIndex = 1~9
    {
        pieceNum--;
        destIndex--;
        curPiece = pieces[pieceNum];

        Vector2 targetPos = pos[destIndex];

        curPiece.GetComponent<RectTransform>().DOAnchorPos(targetPos,0.3f).SetEase(Ease.Linear);
    }

    private IEnumerator ClearCoroutine()
    {
        CanvasSetActive(true);

        Image image = piece_9;
        Color color = Color.white;


        float curTime = 0f;
        while(clearTime > curTime)
        {
            yield return null;
            curTime += Time.deltaTime;
            color.a = curTime / clearTime;
            image.color = color;
        }

        CanvasSetActive(false);
    }
    private void PieceInit()
    {
        Color temp = piece_9.color;
        temp.a = 0f;
        piece_9.color = temp;

        for(int i = 0; i < pieces.Length; i++)
        {
            curPiece = pieces[i];
            curPiece.GetComponent<RectTransform>().anchoredPosition = PieceInitPos[i];
        }
    }

    public void CanvasSetActive(bool active)
    {
        canvas.SetActive(active);
    }
}
