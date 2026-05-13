using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoardPos
{
    public int x;
    public int y;
}

public class PentominoBoard : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 6;
    [SerializeField] private float gridSize = 1f;

    [SerializeField] private BoardPos[] NotValidPos;

    private bool[,] boardData;
    private Vector3[,] boardWorldPos;
    private Dictionary<BoardPos, Vector3> board = new(); // 보드상좌표와 월드포지션을 묶음
    private Dictionary<Vector3, BoardPos> reverseDict = new();

    private Vector3 zeroPos = Vector3.zero;

    public float GridSize => gridSize;
    public Vector3 ZeroPos => zeroPos;

    private void Start()
    {
        gridSize *= transform.lossyScale.x;
        zeroPos = transform.GetChild(0).position;// 첫번째 자식은 무조건 기준포지션
    }
    public void Init()
    {
        
        boardData = new bool[width, height];
        boardWorldPos = new Vector3[width, height];

        MakeBoard();
        SetStartBoard(NotValidPos);
        DebugBoard();
    }
    private void MakeBoard()
    {
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                boardWorldPos[x, y] = new Vector3(zeroPos.x + x * gridSize, zeroPos.y, zeroPos.z + gridSize * y);
                board.Add(new BoardPos() { x = x, y = y }, boardWorldPos[x, y]);
            }
        }
        foreach(var value in board)
        {
            reverseDict.Add(value.Value,value.Key);
        }
    }
    private void SetStartBoard(BoardPos[] notValidPos)
    {
        if(notValidPos != null && notValidPos.Length != 0)
        {
            foreach (BoardPos pos in notValidPos)
            {
                if (width < pos.x || height < pos.y)
                    continue;
                boardData[pos.x - 1, pos.y - 1] = true;
            }
        }

    }
    public Vector3 BoardPosToWorldPos(BoardPos pos)// 보드좌표 입력하면 월드좌표 반환
    {
        return board[pos];
    }
    public BoardPos WorldPosToBoardPos(Vector3 pos)
    {
        Vector3 local = pos - zeroPos;
        int x = Mathf.RoundToInt(local.x / gridSize);
        int y = Mathf.RoundToInt(local.z / gridSize);

        if (x >= 0 && x < width && y >= 0 && y < height)
            return new BoardPos { x = x, y = y };

        return new BoardPos { x = -999, y = -999 }; // invalid 표시
    }

    public void SetActiveBoard(BoardPos[] boardPos, bool newActive)
    {
        foreach (var pos in boardPos)
        {
            if (pos.x >= 0 && pos.x < width &&
                 pos.y >= 0 && pos.y < height)
            {
                boardData[pos.x, pos.y] = newActive;
            }
        }
        if (newActive)
        {
            Debug.Log("=== Piece placed successfully - Current board state ===");
            DebugBoard();
        }
    }

    public bool IsPlace(BoardPos[] worldPos)
    {
        foreach(var pos in worldPos)
        {
            //BoardPos boardIndex = WorldPosToBoardPos(worldPos[i]); //Todo

            // 범위 벗어나면 못 놓게 막기 (IndexOutOfRange 방지!)
            if (pos.x < 0 || pos.x >= width ||
                pos.y < 0 || pos.y >= height)
            {
                return false;// Out of board bounds
            }

            if (boardData[pos.x, pos.y])
            {
                return false;// Position already occupied
            }
        }
        return true;
    }

    public void IsGameClearCheck()
    {
        foreach(bool InPlace in boardData)
        {
            if (!InPlace)
                return;
        }
        EventBus.PublishPentominoClear();
    }
    public void DebugBoard()
    {
        Debug.Log("========== board  ==========");

        for (int y = height - 1; y >= 0; y--)           // 행 (Y축, 위→아래)
        {
            string row = $"Row {y:00} | ";         // 행 번호 표시
            for (int x = 0; x < width; x++)        // 열 (X축, 왼→오른)
            {
                row += boardData[x, y] ? " O " : " X ";
            }
            Debug.Log(row);
        }

        Debug.Log("=================================================");
    }
}
