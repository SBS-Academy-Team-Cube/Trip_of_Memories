using UnityEngine;

public class BinaryPlayer : MonoBehaviour,IBinaryLandMapObject
{
    [SerializeField] private BinaryUIMng UiMng;

    [SerializeField] private float WidthGridSize;
    [SerializeField] private float HeightGridSize;
    [SerializeField] private bool isReverse;


    private Vector2Int curGrid; // 한 칸이 1인 좌표값
    private RectTransform curTransform; // 트랜스폼값

    public Vector2Int CurGrid => curGrid;

    public void GameStart()
    {
        if(!isReverse)
        {
            curGrid = new Vector2Int(9, 0);

            curTransform = GetComponent<RectTransform>();
            OnMove(curGrid);
        }
        else
        {
            curGrid = new Vector2Int(11, 0);

            curTransform = GetComponent<RectTransform>();
            OnMove(curGrid);
        }
    }

    public void OnMove(Vector2Int targetGrid)
    {
        curTransform.anchoredPosition = new Vector2(WidthGridSize * targetGrid.x, HeightGridSize * targetGrid.y);
    }
    public bool IsMove(Vector2Int curGrid, Vector2 dir)
    {
        if(!isReverse) // 정방향
        {
            return UiMng.isMove(curGrid, dir);
        }
        else
        {
            return UiMng.isMove(curGrid, new Vector2(-dir.x, dir.y));
        }
    }

    public void TryMove(Vector2 dir) // 모든 이동 조건 검사하고 OnMove호출
    {
        if (dir == Vector2.zero)
            return;
        if (!IsMove(curGrid, dir))
            return;


        if(dir == Vector2.up)
        {
            curGrid.y++;
            OnMove(curGrid);
        }
        else if(dir == Vector2.down)
        {
            curGrid.y--;
            OnMove(curGrid);
        }
        else if(dir == Vector2.left)
        {
            if(isReverse)
                curGrid.x++;
            else
                curGrid.x--;
            OnMove(curGrid);
        }
        else if(dir == Vector2.right)
        {
            if(isReverse)
                curGrid.x--;
            else
                curGrid.x++;
            OnMove(curGrid);
        }

    }

}
