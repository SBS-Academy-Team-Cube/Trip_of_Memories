using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SlidePuzzleUIMng : MonoBehaviour
{
    [Header("piecePosition")]
    [SerializeField] private RectTransform[] GridPos;

    [Header("pieces")]
    [SerializeField] private Image[] pieces;

    [Header("Canvas")]
    [SerializeField] private GameObject canvas;
    // 실제 UI상에서 조각 이동하는 역할..
    private Vector2[] startPos;
    private Vector2[] pos;

    private Image curPiece;


    public void GameInit(SlidePuzzleBoard board)
    {
        if (startPos == null)
            StartSet();

        //// 현재 보드 상태대로 조각 배치
        //for (int i = 0; i < 9; i++)
        //{
        //    SlidePuzzlePieceType type = board.GetPieceTypeAt(i + 1);

        //    if (type == SlidePuzzlePieceType.Empty)
        //        continue;

        //    // 해당 타입의 Piece 찾기
        //    for (int j = 0; j < pieces.Length; j++)
        //    {
        //        if (pieces[j].GetComponent<SlidePuzzlePiece>().PieceType == type)
        //        {
        //            pieces[j].GetComponent<RectTransform>().anchoredPosition = startPos[i];
        //            break;
        //        }
        //    }
        //}

        //canvas.SetActive(true);
    }
    public void GameClear()
    {
        canvas.SetActive(false);    
    }
    private void StartSet()
    {
        startPos = new Vector2[GridPos.Length];
        for (int i = 0; i < GridPos.Length; i++)
        {
            startPos[i] = new Vector2(GridPos[i].anchoredPosition.x, GridPos[i].anchoredPosition.y);
        }
    }
    public void MovePiece(int pieceNum, int destIndex)// pieceNum = 1~8 , destIndex = 1~9
    {
        pieceNum--;
        destIndex--;

        curPiece = pieces[pieceNum];

        Vector2 targetPos = pos[destIndex];

        curPiece.GetComponent<RectTransform>().DOAnchorPos(targetPos,0.3f).SetEase(Ease.Linear);
    }



}
