using UnityEngine;
using UnityEngine.UI;

public class BinaryUIMng : MonoBehaviour // 플레이어 이동을 제외한 맵의 오브젝트들을 관리
{
    [SerializeField] private GameObject[] PushBoxs;// 플레이어가 밀면 이동해야함
    [SerializeField] private GameObject[] Enemys; // 플레이어가 공격하면 사라지기만 하면 됨
    [SerializeField] private Vector2Int SwitchPos; // 플레이어가 해당 스위치를 밟았는지만 체크하면됨
    [SerializeField] private GameObject SwitchWall; // 스위치가 밟히면 사라지면됨

    [SerializeField] private BinaryPlayer player1;
    [SerializeField] private BinaryPlayer player2;
    [SerializeField] private BinaryLandGameMng gameMng;

    [SerializeField] private GameObject MainCanvas;


    private RoroTileType[] MapGrid;
    private int width = 21;
    private int height = 12;

    private Vector2Int clearPos1 = new Vector2Int(9, 10);
    private Vector2Int clearPos2 = new Vector2Int(11, 10);

    private int mapIndex = -1;


    public void SaveMap(RoroLevelData levelData,int mapIndex)
    {
        MapGrid = levelData.tiles;
        this.mapIndex = mapIndex;
    }
    public bool isMove(Vector2Int curGrid, Vector2 dir)// 플레이어 현재위치, 방향키 입력값
    {
        Vector2Int TargetGrid = Vector2Int.zero;

        if (dir == Vector2.up && curGrid.y < height)
            TargetGrid = new Vector2Int(curGrid.x, curGrid.y + 1);
        else if (dir == Vector2.down && curGrid.y >= 0)
            TargetGrid = new Vector2Int(curGrid.x, curGrid.y - 1);
        else if(dir == Vector2.left && curGrid.x >= 0)
            TargetGrid = new Vector2Int(curGrid.x - 1, curGrid.y);
        else if (dir == Vector2.right && curGrid.x < width)
            TargetGrid = new Vector2Int(curGrid.x + 1, curGrid.y);


        int TargetIndex = GridToIndex(TargetGrid);

        if (TargetGrid == SwitchPos)
            SwitchOn();

        if (MapGrid[TargetIndex] != RoroTileType.Wall && MapGrid[TargetIndex] != RoroTileType.SwitchWall)
            return true;
        return false;
    }


    private int GridToIndex(Vector2Int Grid)
    {
        return Grid.y * width + Grid.x;
    }
    private void SwitchOn()
    {
        for(int i = 0; i< MapGrid.Length; i++)
        {
            if (MapGrid[i] == RoroTileType.SwitchWall)
                MapGrid[i] = RoroTileType.Empty;
        }
        SwitchWall.SetActive(false);
    }


    public void ClearCheck()
    {
        if (player1.CurGrid == clearPos1 && player2.CurGrid == clearPos2)
        {
            gameMng.MapClear(mapIndex);
        }

        if (player1.CurGrid == clearPos2 && player1.CurGrid == clearPos1)
        {
            gameMng.MapClear(mapIndex);
        }

    }

    public void GameClear()
    {
        MainCanvas.SetActive(false);
    }

}
