using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SlidePuzzleUIMng : MonoBehaviour
{
    [Header("piecePosition")]
    [SerializeField] private RectTransform[] pos;


    [Header("pieces")]
    [SerializeField] private Image[] pieces;
    // 실제 UI상에서 조각 이동하는 역할..

    private Image curPiece;

    public void MovePiece(int pieceNum, int destIndex)// pieceNum = 1~8 , destIndex = 1~9
    {
        pieceNum--;
        destIndex--;
        curPiece = pieces[pieceNum];

        Vector2 targetPos = new Vector2(pos[destIndex].anchoredPosition.x, pos[destIndex].anchoredPosition.y);

        curPiece.GetComponent<RectTransform>().DOAnchorPos(targetPos,0.8f).SetEase(Ease.Linear);
    }
}
