using UnityEngine;
using UnityEngine.UI;

public class SlidePuzzleUIMng : MonoBehaviour
{
    [Header("piecePosition")]
    [SerializeField] private int posX1;
    [SerializeField] private int posX2;
    [SerializeField] private int posX3;
    [SerializeField] private int posY1;
    [SerializeField] private int posY2;
    [SerializeField] private int posY3;

    [Header("pieces")]
    [SerializeField] private Image[] pieces;
    // 실제 UI상에서 조각 이동하는 역할..

    public void MovePiece(int pieceNum, int destIndex)
    {
        Image piece = pieces[pieceNum];
        int destX = int.MaxValue;
        int destY = int.MaxValue;
        if(destIndex == 0)
        {
            switch(destIndex)
            {
                case 1:
                    destX = posX1;
                    destY = posY1;
                    break;
                case 2:
                    destX = posX2;
                    destY = posY1;
                    break;
                case 3:
                    destX = posX3;
                    destY = posY1;
                    break;
                case 4:
                    destX = posX1;
                    destY = posY2;
                    break;
                case 5:
                    destX = posX2;
                    destY = posY2;
                    break;
                case 6:
                    destX = posX3;
                    destY = posY2;
                    break;
                case 7:
                    destX = posX1;
                    destY = posY3;
                    break;
                case 8:
                    destX = posX2;
                    destY = posY3;
                    break;
                    case 9:
                    destX = posX3;
                    destY = posY3;
                    break;
                default:
                    Debug.LogError("destIndex error");
                    break;
            }
        }
        if(destX != int.MaxValue && destY != int.MaxValue)
        {
            piece.rectTransform.anchoredPosition = new Vector2(destX, destY);
        }
    }
}
