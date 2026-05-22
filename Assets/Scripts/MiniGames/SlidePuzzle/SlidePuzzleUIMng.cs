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

    [Header("minigameCanvas")]
    [SerializeField] private GameObject canvas;

    [Header("last Piece")]
    [SerializeField] private GameObject piece_9;

    // 실제 UI상에서 조각 이동하는 역할..
    private Vector2[] pos;

    private Image curPiece;

    public void GameInit()
    {
        if (GridPos == null)
            return;
        pos = new Vector2[GridPos.Length];
        for(int i = 0; i < GridPos.Length; i++)
        {
            pos[i] = new Vector2(GridPos[i].anchoredPosition.x, GridPos[i].anchoredPosition.y);
        }

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

        Image image = piece_9.GetComponent<Image>();
        Color color = Color.white;


        float curTime = 0f;
        float time = 1f;
        while(time > curTime)
        {
            yield return null;
            curTime += Time.deltaTime;
            color.a = curTime;
            image.color = color;
        }

        CanvasSetActive(false);
    }

    public void CanvasSetActive(bool active)
    {
        canvas.SetActive(active);
    }
}
