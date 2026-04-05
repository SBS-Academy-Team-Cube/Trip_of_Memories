using UnityEngine;

[CreateAssetMenu(menuName = "Pentomino/Shape")]
public class PieceShape : ScriptableObject
{
    public BoardPos[] shape;  // ← 여기다 좌표 넣음
}
