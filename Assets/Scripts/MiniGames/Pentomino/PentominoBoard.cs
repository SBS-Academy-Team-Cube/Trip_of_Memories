using UnityEngine;

[System.Serializable]
public struct BoardPos
{
    public int x;
    public int y;
}

public class PentominoBoard : MonoBehaviour
{
    private int width = 10;
    private int height = 6;

    private bool[,] board;

    private BoardPos zeroPos = new BoardPos();

    private void Awake()
    {
        InitBoard();
    }
    private void InitBoard()
    {
        board = new bool[width, height];
        zeroPos.x = 4;
        zeroPos.y = 3;
        DebugBoard();
    }
    private BoardPos PosToBoardPos(BoardPos pos)// Convert world coordinate to array index
    {
        BoardPos newPos;
        newPos.x = pos.x + zeroPos.x;
        newPos.y = pos.y + zeroPos.y;
        return newPos;
    }

    public void SetActiveBoard(BoardPos[] boardPos, bool newActive)
    {
        foreach (var pos in boardPos)
        {
            BoardPos newPos = PosToBoardPos(pos);
            if (newPos.x >= 0 && newPos.x < width &&
                 newPos.y >= 0 && newPos.y < height)
            {
                board[newPos.x, newPos.y] = newActive;
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
        for (int i = 0; i < worldPos.Length; ++i)
        {
            BoardPos boardIndex = PosToBoardPos(worldPos[i]);

            // 범위 벗어나면 못 놓게 막기 (IndexOutOfRange 방지!)
            if (boardIndex.x < 0 || boardIndex.x >= width ||
                boardIndex.y < 0 || boardIndex.y >= height)
            {
                return false;// Out of board bounds
            }

            if (board[boardIndex.x, boardIndex.y])
            {
                return false;// Position already occupied
            }
        }
        return true;
    }

    public void IsGameClearCheck()
    {
        foreach(bool InPlace in board)
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
                row += board[x, y] ? " O " : " X ";
            }
            Debug.Log(row);
        }

        Debug.Log("=================================================");
    }
}
