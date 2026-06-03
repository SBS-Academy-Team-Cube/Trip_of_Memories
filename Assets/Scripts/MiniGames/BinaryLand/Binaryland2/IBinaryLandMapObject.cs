using UnityEngine;

public interface IBinaryLandMapObject
{
    public Vector2Int CurGrid { get; }
    public void TryMove(Vector2 dir);
    public void OnMove(Vector2Int targetGrid);
    public bool IsMove(Vector2Int curGrid, Vector2 dir);
}
