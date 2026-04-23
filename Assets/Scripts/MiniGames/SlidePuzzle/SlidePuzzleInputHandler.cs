using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlidePuzzleInputHandler : MonoBehaviour
{
    [SerializeField] private SlidePuzzlePiece[] piece;//(0~7)
    [SerializeField] private SlidePuzzleBoard board;
    [SerializeField] private SlidePuzzleUIMng UIMng;
    //조각이 클릭되었을때 보드를 참조해서 상하좌우 중 빈 칸 있는지 체크..
    private SlidePuzzlePiece curPiece;


    private void Awake()
    {
        if(piece.Length != 8)
        {
            Debug.LogError("pieces length error");
        }
        if(board == null)
        {
            Debug.LogError("board is null");
        }
    }
    private void OnEnable()
    {
        // for inspector add listener button
    }

    public void OnPieceButtonClicked(int pieceNum)//1~8
    {
        //Debug.Log($"{pieceNum}");//8?
        //return; // 테스트용
        curPiece = piece[--pieceNum];
        int EmptyIndex = board.IsMove(curPiece.CurPlaceIndex); // curPiece가 이동해야하는 인덱스
        Debug.Log($"empty Index => {EmptyIndex}");
        if (EmptyIndex == 0)// 이동 불가능한 조각임...
            return;

        board.SetBoardIndex(curPiece.CurPlaceIndex, SlidePuzzlePieceType.Empty);// 현재위치 비우고
        board.SetBoardIndex(EmptyIndex, curPiece.PieceType);// 이동할 위치에 조각 넣고

        curPiece.SetPlaceIndex(EmptyIndex);// 조각의 현재 위치 업데이트
        
        UIMng.MovePiece(++pieceNum, EmptyIndex);//ui상 위치 실제이동
        board.DebugBoard();

        if(board.ClearCheck())
        {
            //  send Event for GameMng
            Debug.Log("Game Clear");
        }
    }
}
