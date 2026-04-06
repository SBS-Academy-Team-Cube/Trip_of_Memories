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
        board = new bool[height, width];
        zeroPos.x = 4;
        zeroPos.y = 3;
        DebugBoard();
    }
    private BoardPos PosToBoardPos(BoardPos pos)
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
            if (newPos.x >= 0 && newPos.x < height &&
                 newPos.y >= 0 && newPos.y < width)
            {
                board[newPos.x, newPos.y] = newActive;
            }
        }
    }

    public bool IsPlace(BoardPos[] worldPos)
    {
        for (int i = 0; i < worldPos.Length; ++i)
        {
            BoardPos boardIndex = PosToBoardPos(worldPos[i]);

            Debug.Log($"[IsPlace] World({worldPos[i].x}, {worldPos[i].y}) → BoardIndex({boardIndex.x}, {boardIndex.y})");

            // 범위 벗어나면 못 놓게 막기 (IndexOutOfRange 방지!)
            if (boardIndex.x < 0 || boardIndex.x >= height ||
                boardIndex.y < 0 || boardIndex.y >= width)
            {
                return false;
            }

            if (board[boardIndex.x, boardIndex.y])
            {
                return false;
            }
        }
        return true;
    }

    public void DebugBoard()
    {
        for(int i = 0; i < height; ++i)
        {
            for(int  j = 0; j < width; ++j)
            {
                Debug.Log($"{i} - {j} : {board[i,j]}");
            }
        }
    }
}
