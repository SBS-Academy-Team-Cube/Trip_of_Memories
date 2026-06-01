using UnityEngine;

public class BinaryPlayer : MonoBehaviour
{
    [SerializeField] private float WidthGridSize;
    [SerializeField] private float HeightGridSize;
    [SerializeField] private bool isReverse;


    private Vector2Int curGrid;
    private RectTransform curTransform;

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

    private void OnMove(Vector2Int targetGrid)
    {
        curTransform.anchoredPosition = new Vector2(WidthGridSize * targetGrid.x, HeightGridSize * targetGrid.y);
    }

    public void TryMove(Vector2 dir)
    {
        if (dir == Vector2.zero)
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
