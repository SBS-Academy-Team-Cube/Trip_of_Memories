using UnityEngine;

public class BinaryLandMapEditor : MonoBehaviour
{
    [SerializeField] private RoroLevelData[] mapData;

    [SerializeField] private Transform gridTrans; // panel trans

    [Header("tile Prefab")]
    [SerializeField] private GameObject EmptyPrefab;
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject KeyPrefab;
    [SerializeField] private GameObject SwitchPrefab;
    [SerializeField] private GameObject ExitPrefab;


    private const int width = 21;
    private const int height = 12;


    private RoroLevelData curData;
    


    public void CreateMap(int index)
    {
        curData = mapData[index];


    }
}
