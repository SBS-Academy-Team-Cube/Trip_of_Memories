using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SlidePuzzleUIMng : MonoBehaviour
{
    [Header("piecePosition")]
    [SerializeField] private RectTransform[] GridPos;

    [Header("pieces")]
    [SerializeField] private Image[] pieces;
    // 실제 UI상에서 조각 이동하는 역할..
    private Vector2[] pos;

    private Image curPiece;

    private void Start()
    {
        if (GridPos == null)
            return;
        pos = new Vector2[GridPos.Length];
        for(int i = 0; i < GridPos.Length; i++)
        {
            pos[i] = new Vector2(GridPos[i].anchoredPosition.x, GridPos[i].anchoredPosition.y);
        }
    }
    public void MovePiece(int pieceNum, int destIndex)// pieceNum = 1~8 , destIndex = 1~9
    {
        pieceNum--;
        destIndex--;
        curPiece = pieces[pieceNum];

        Vector2 targetPos = pos[destIndex];

        curPiece.GetComponent<RectTransform>().DOAnchorPos(targetPos,0.8f).SetEase(Ease.Linear);
    }
}
