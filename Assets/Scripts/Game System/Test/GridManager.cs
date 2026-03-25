using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int Row = 6;
    public int Column = 3;
    public Vector3 StartPosition;
    public RotatingTile TilePrefab;
    public float TileDistance = 14.5f;
    private RotatingTile[,] Grid;

    void Start()
    {
        CreateGrid();
    }
    private void CreateGrid()
    {
        Grid = new RotatingTile[Row, Column];
        for (int row = 0; row < Row; row++)
        {
            for (int col = 0; col < Column; col++)
            {
                Vector3 SpawnPosition = StartPosition + new Vector3(col * TileDistance, 0, row * TileDistance);
                Grid[row, col] = Instantiate(TilePrefab, SpawnPosition, Quaternion.identity);
                Grid[row, col].Init(Random.Range(0, 4), (ETileType)Random.Range(0, 3));
            }
        }
    }
}
