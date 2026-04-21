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
    private SlidePuzzlePieceType[,] board;
    private int boardSize = 3;

    private void Awake()
    {
        board = new SlidePuzzlePieceType[boardSize, boardSize];
        InitBoard();
    }
    private void InitBoard()
    {
        int index = 0;
        //
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                board[x, y] = initBoardSetting[index].pieceType;
            }
        }
        //{(0,0) , (0,1) , (0,2)}
        //{(1,0) , (1,1) , (1,2)}
        //{(2,0) , (2,1) , (2,2)}
    }


    public int IsMove(int index)// return empty index
    {
        //1-> (0,0), 2->(1,0), 3->(2,0), 4->(0,1), 5->(1,1), 6->(2,1), 7->(0,2), 8->(1,2), 9->(2,2)
        int x = (index - 1) % boardSize;
        int y = (index - 1) / boardSize;
        if(x < 0 || x >= boardSize || y < 0 || y >= boardSize)
        {
            Debug.LogError("index error");
            return 0;
        }

        if (y > 0)// 왼쪽체크
        {
            if (board[x, y - 1] == SlidePuzzlePieceType.Empty)
                return index - 1;
        }
        if (y < boardSize - 1)// 오른쪽체크
        {
            if(board[x, y + 1] == SlidePuzzlePieceType.Empty)
                return index + 1;
        }
        if (x > 0)// 위쪽체크
        {
            if(board[x - 1, y] == SlidePuzzlePieceType.Empty)
                return index - boardSize;
        }
        if (x < boardSize - 1)// 아래쪽체크
        {
            if(board[x + 1, y] == SlidePuzzlePieceType.Empty)
                return index + boardSize;
        }

        return 0;
    }

    public bool ClearCheck()
    {
        for(int i = 0; i < board.Length; ++i)
        {
            int x = i % boardSize;
            int y = i / boardSize;

            if(!(board[x,y] == (SlidePuzzlePieceType)i + 1))// index 0 => piece1 
            {
                return false;
            }
            if(i == board.Length - 1)// last index empty 
            {
                return board[x, y] == SlidePuzzlePieceType.Empty;
            }
        }
        return false;
    }
    public void SetBoardIndex(int index, SlidePuzzlePieceType pieceType)
    {
        int x = (index - 1) % boardSize;
        int y = (index - 1) / boardSize;
        if (x < 0 || x >= boardSize || y < 0 || y >= boardSize)
        {
            Debug.LogError("index error");
            return;
        }
        board[x, y] = pieceType;
    }
    public void DebugBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                Debug.Log($"board[{x},{y}] : {board[x, y]}");
            }
        }
    }
}
