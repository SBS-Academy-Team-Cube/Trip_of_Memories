using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EDirection
{
    None  = 0,
    Up    = 1 << 0,
    Right = 1 << 1,
    Down  = 1 << 2,
    Left  = 1 << 3
}
public class Cell
{
    public bool visited;
    public EDirection directions = EDirection.None;
}

public class GridManager : MonoBehaviour
{
    public int row = 3;
    public int column = 3;

    private Cell[,] cellGrid;

    private Vector2Int[] dirs = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    
    private List<Vector2Int> path = new List<Vector2Int>();

    void Start()
    {
        GenerateLoop(new Vector2Int(0, 0));
    }
    public void GenerateLoop(Vector2Int start)
    {
        cellGrid = new Cell[row, column];

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                cellGrid[r, c] = new Cell();
            }
        }

        DFS(start);

        BuildConnections();
    }
    private bool DFS(Vector2Int current)
    {
        cellGrid[current.x, current.y].visited = true;
        path.Add(current);

        if (path.Count == row * column)
        { 
            return true;
        }

        List<Vector2Int> shuffled = [.. dirs];
        Shuffle(shuffled);

        foreach (var dir in shuffled)
        {
            Vector2Int next = current + dir;

            if (!OOB(next)) continue;
            if (cellGrid[next.x, next.y].visited) continue;

            if (DFS(next))
                return true;
        }

        path.RemoveAt(path.Count - 1);
        return false;
    }
    private void BuildConnections()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int current = path[i];
            Vector2Int next = path[(i + 1) % path.Count];

            Vector2Int dir = next - current;

            AddConnection(current, next, dir);
        }
    }

    private void AddConnection(Vector2Int a, Vector2Int b, Vector2Int dir)
    {
        EDirection d = DirToEnum(dir);
        EDirection opposite = Opposite(d);

        cellGrid[a.x, a.y].directions |= d;
        cellGrid[b.x, b.y].directions |= opposite;
    }
    private bool OOB(Vector2Int p)
    {
        return p.x >= 0 && p.x < row && p.y >= 0 && p.y < column;
    }
    private void Shuffle(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
    private EDirection DirToEnum(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return EDirection.Up;
        if (dir == Vector2Int.right) return EDirection.Right;
        if (dir == Vector2Int.down) return EDirection.Down;
        if (dir == Vector2Int.left) return EDirection.Left;
        return EDirection.None;
    }

    private EDirection Opposite(EDirection dir)
    {
        switch (dir)
        {
            case EDirection.Up: return EDirection.Down;
            case EDirection.Down: return EDirection.Up;
            case EDirection.Left: return EDirection.Right;
            case EDirection.Right: return EDirection.Left;
        }
        return EDirection.None;
    }
}