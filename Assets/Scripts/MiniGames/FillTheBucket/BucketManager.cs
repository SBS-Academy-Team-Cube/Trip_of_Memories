using UnityEngine;
using UnityEngine.UI;

public class BucketManager : MonoBehaviour
{
    [Header("BucketButton")]
    [SerializeField] private Button Button_3L;
    [SerializeField] private Button Button_7L;
    [SerializeField] private Button Button_5L;

    [Header("Bucket")]
    [SerializeField] private Bucket Bucket_3L;
    [SerializeField] private Bucket Bucket_7L;
    [SerializeField] private Bucket Bucket_5L;

    [SerializeField] private WaterGameManager BucketGameManager;

    private IBucket CurBucket = null;
    private IBucket PrevBucket = null;

    private void Awake()
    {
        Button_3L.onClick.AddListener(() => SelectBucket(Bucket_3L));
        Button_5L.onClick.AddListener(() => SelectBucket(Bucket_5L));
        Button_7L.onClick.AddListener(() => SelectBucket(Bucket_7L));

        Bucket_5L.ClearEvent.AddListener(() => GameClear());
        Bucket_5L.FailEvent.AddListener(() => ResetGame());
    }

    public void Init()
    {
        Bucket_3L.Init();
        Bucket_7L.Init();
        Bucket_5L.Init();
    }

    private void SelectBucket(IBucket bucket)// Save the last selected bucket
    {
        if (bucket == null) return;
        PrevBucket = CurBucket;
        CurBucket = bucket;
    }

    public void FillBucket()
    {
        if (CurBucket == null || !CurBucket.CanBeFilled) return;// 5L and 7L buckets cannot be filled

        CurBucket.SetWaterAmount(CurBucket.MaxCapacity());
    }

    public void EmptyBucket()
    {
        if (CurBucket == null) return;
        CurBucket.SetWaterAmount(0f);
    }

    public void MoveWater()
    {
        if (CurBucket == null || PrevBucket == null) return;

        // Fill with water and get the actual amount filled
        float temp = CurBucket.AddWater(PrevBucket.CurrentWater());
        // Remove as much as was filled
        PrevBucket.AddWater(-temp);

    }

    private void GameClear()
    {
        BucketGameManager.GameClear();
    }
    private void ResetGame()
    {
        BucketGameManager.GameReset();
    }
}
