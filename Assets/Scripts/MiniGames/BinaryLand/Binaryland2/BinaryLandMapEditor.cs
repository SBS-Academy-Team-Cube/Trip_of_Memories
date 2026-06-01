using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class BinaryLandMapEditor : MonoBehaviour
{
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

    private void Start()
    {
        GameStart();
    }
    public void GameStart()
    {
        if (tileMap == null)
            CreateGrid();

        CreateMap(0);
    }

    private void CreateGrid()
    {
        if (tilePrefab == null)
            return;

        foreach(Transform grid in gridTrans)
        {
            Destroy(grid.gameObject); // 초기화.. 지우기
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

    public void CreateMap(int index)
    {
        if (index < 0 || index >= mapData.Length)
            return;

        curData = mapData[index];
        //curData.ReSize();
        DrawMap();
    }

    private void DrawMap()
    {
        if (curData == null)
            return;

        int index = 0;

        for(int y = 0;y < height; y++)
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
