using UnityEngine;

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
        zeroPos.x = -4;
        zeroPos.y = -3;
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

        foreach(var pos in boardPos)
        {
            BoardPos newPos = PosToBoardPos(pos);
            board[newPos.x, newPos.y] = newActive;
        }
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
