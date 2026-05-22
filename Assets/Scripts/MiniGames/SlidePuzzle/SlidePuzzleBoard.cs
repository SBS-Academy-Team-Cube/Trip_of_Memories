using System;
using UnityEngine;

public enum SlidePuzzlePieceType
{
    Empty,
    Piece1,
    Piece2,
    Piece3,
    Piece4,
    Piece5,
    Piece6,
    Piece7,
    Piece8
}
[Serializable]
public struct SlidePuzzleBoardGrid
{
    public int index;
    public SlidePuzzlePieceType pieceType;
}
public class SlidePuzzleBoard : MonoBehaviour
{
    [SerializeField] private SlidePuzzleBoardGrid[] initBoardSetting;
    //조각이 다 맞춰졌는지 체크
    // 3X3 칸 위치정보 저장
    private SlidePuzzlePieceType[] board;
    private int indexCount = 9;

    public void GameInit()
    {
        board = new SlidePuzzlePieceType[indexCount];
        InitBoard();
    }
    private void InitBoard()
    {
        int index = 0;
        //
        for(int i = 0; i < indexCount; i++)
        {
            board[i] = initBoardSetting[index].pieceType;
            index++;
        }
        DebugBoard();
    }


    public int IsMove(int index)// index = 1~9 , return empty index
    {
        if(index < 1 || index > indexCount)
        {
            Debug.LogError("index error");
            return 0;
        }
        int boardIndex = index - 1;
        
        if (index % 3 != 1)// 왼쪽체크
        {
            if (board[boardIndex - 1] == SlidePuzzlePieceType.Empty)
                return index - 1;
        }
        if (index % 3 != 0)// 오른쪽체크
        {
            if(board[boardIndex + 1] == SlidePuzzlePieceType.Empty)
                return index + 1;
        }
        if (index > 3)// 위쪽체크
        {
            if(board[boardIndex - 3] == SlidePuzzlePieceType.Empty)
                return index - 3;
        }
        if (index < 7)// 아래쪽체크
        {
            if(board[boardIndex + 3] == SlidePuzzlePieceType.Empty)
                return index + 3;
        }

        return 0;
    }

    public bool ClearCheck()
    {
        for(int i = 1; i <= indexCount; ++i)
        {

            if (i == indexCount && board[i - 1] == SlidePuzzlePieceType.Empty)
                return true;

            if (board[i - 1] != (SlidePuzzlePieceType)i)
            {
                Debug.Log($"{i} clear fail");
                break;
            }

        }
        return false;
    }
    public void SetBoardIndex(int index, SlidePuzzlePieceType pieceType)
    {
        if (index < 1 || index > indexCount)
        {
            Debug.LogError("index error");
            return;
        }
        board[index - 1] = pieceType;
    }

    public SlidePuzzlePieceType GetPieceTypeAt(int index) // index 1~9
    {
        if (index < 1 || index > 9) return SlidePuzzlePieceType.Empty;
        return board[index - 1];
    }

    public void DebugBoard()
    {
        Debug.Log("===============================");
        for(int i = 0; i < indexCount; ++i)
        {
            Debug.Log($"{board[i]} => index {i + 1}");
        }
    }
}
