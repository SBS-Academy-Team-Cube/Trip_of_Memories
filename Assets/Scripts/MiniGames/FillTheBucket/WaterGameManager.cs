using UnityEngine;
using UnityEngine.UI;

public class WaterGameManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button FillButton;
    [SerializeField] private Button EmptyButton;
    [SerializeField] private Button MoveButton;
    [SerializeField] private Button ExitButton;

    [SerializeField] private BucketManager BucketManager;
    [SerializeField] private Canvas BucketGameMainCanvas;

    private void Awake()
    {
        FillButton.onClick.AddListener(() => BucketManager.FillBucket());
        EmptyButton.onClick.AddListener(() => BucketManager.EmptyBucket());
        MoveButton.onClick.AddListener(() => BucketManager.MoveWater());
        ExitButton.onClick.AddListener(() => ExitWaterGame());
        
        Init();
    }

    public void Init()
    {
        BucketManager.Init();
    }

    private void ExitWaterGame()
    {
        BucketGameMainCanvas.gameObject.SetActive(false);
    }
    public void GameClear()
    {
        Debug.Log("GameClear...");
        BucketGameMainCanvas.gameObject.SetActive(false);
    }
    public void GameReset()
    {
        Debug.Log("Game Fail... Reset");
        Init();
    }
}
