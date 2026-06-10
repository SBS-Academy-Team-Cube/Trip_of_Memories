using UnityEngine;
using UnityEngine.UI;

public class BinaryLandMapEditor : MonoBehaviour
{
    [SerializeField] private BinaryUIMng uimng;

    [SerializeField] private RoroLevelData[] mapData;

    [SerializeField] private Transform gridTrans; // panel trans

    [Header("tile Prefab")]
    [SerializeField] private Image tilePrefab;

    [Header("tile Image")]
    [SerializeField] private Sprite EmptyImage;
    [SerializeField] private Sprite WallImage;
    [SerializeField] private Sprite PlayerImage;
    [SerializeField] private Sprite EnemyImage;
    [SerializeField] private Sprite KeyImage;
    [SerializeField] private Sprite SwitchImage;
    [SerializeField] private Sprite SwitchWallImage;
    [SerializeField] private Sprite ExitImage;
    [SerializeField] private Sprite BoxImage;


    private const int width = 21;
    private const int height = 12;


    private RoroLevelData curData;
    private Image[,] tileMap;


    public void GameStart(int MapIndex)
    {
        if (tileMap == null)
            CreateGrid();

        if(MapIndex < 0 ||  MapIndex >= mapData.Length)
        {
            Debug.Log("index error");
            return;
        }
        CreateMap(MapIndex);
    }

    private void CreateGrid()
    {
        if (tilePrefab == null)
            return;

        foreach(Transform grid in gridTrans)
        {
            Destroy(grid.gameObject); // �ʱ�ȭ.. �����
        }

        tileMap = new Image[height,width];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Image tile = Instantiate(tilePrefab, gridTrans);
                tileMap[y, x] = tile;
            }
        }
        Debug.Log("grid create");
    }

    private void CreateMap(int index)
    {
        if (index < 0 || index >= mapData.Length)
            return;

        curData = mapData[index];
        //curData.ReSize();
        DrawMap();
        uimng.SaveMap(curData,index);
    }

    private void DrawMap()
    {
        if (curData == null)
            return;

        int index = 0;

        for(int y = height - 1; y >= 0; y--)
        {
            for (int x = 0;x < width; x++)
            {
                if (index >= curData.tiles.Length)
                    return;

                RoroTileType type = curData.tiles[index];
                Sprite sprite = GetSprite(type);

                if(sprite != null)
                    tileMap[y, x].sprite = sprite;


                index++;
            }
        }
        Debug.Log("Map load");
    }
    public void ClearTile(int index)
    {
        
    }
    private Sprite GetSprite(RoroTileType type)
    {
        switch(type)
        {
            case RoroTileType.Empty: return EmptyImage;
            case RoroTileType.Enemy: return EnemyImage;
            case RoroTileType.Wall: return WallImage;
            //case RoroTileType.Player: return PlayerImage;
            case RoroTileType.Key: return KeyImage;
            case RoroTileType.Exit: return ExitImage;
            case RoroTileType.PushBox: return BoxImage;
            case RoroTileType.Switch: return SwitchImage;
            case RoroTileType.SwitchWall: return SwitchWallImage;

            default: return EmptyImage;
        }
    }


}
