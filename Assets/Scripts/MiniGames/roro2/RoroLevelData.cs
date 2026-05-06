using UnityEngine;

public enum RoroTileType
{
    Empty,
    Wall,
    PushBox,
    Enemy,
    Key,
    Ice,
    Water,
    Exit,
    StartPoint,
    LaserStatue,
    Player,
    Player2
}



[CreateAssetMenu(fileName = "LevelData", menuName = "roro/LevelData")]
public class RoroLevelData : ScriptableObject
{
    public int width = 10;
    public int height = 10;

    [HideInInspector] public RoroTileType[] tiles;

    public Vector2Int playerStart;
    public Vector2Int exitPos;

    public void ReSize()
    {
        if(tiles == null || tiles.Length != width *  height)
            tiles = new RoroTileType[width * height];
    }


}
