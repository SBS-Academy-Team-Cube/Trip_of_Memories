using UnityEngine;

public class SlidePuzzlePiece : MonoBehaviour
{
    [SerializeField] private SlidePuzzlePieceType pieceType; // 1~8
    [SerializeField][Range(1,9)] private int StartPlaceIndex; // 1~9
    //조각이 가지고있어야할 정보들만 저장...?
    private int curPlaceIndex;
    public int CurPlaceIndex => curPlaceIndex;
    public SlidePuzzlePieceType PieceType => pieceType;

    public void GameInit()
    {
        curPlaceIndex = StartPlaceIndex;
        Debug.Log($"{gameObject.name} => index {curPlaceIndex}");
    }
    public void SetPlaceIndex(int index)// 1~9
    {
        if(index < 1 || index > 9)
        {
            Debug.LogError("index error");
            return;
        }
        curPlaceIndex = index;
        Debug.Log($"{gameObject.name} => index {curPlaceIndex}");
    }
    public bool IsClear()//현재 위치가 정답위치가 맞는지 체크
    {
        return curPlaceIndex == (int)pieceType;
    }
}
