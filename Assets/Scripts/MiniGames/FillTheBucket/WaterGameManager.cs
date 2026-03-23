using UnityEngine;
using UnityEngine.UI;

public class WaterGameManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button HowButton;
    [SerializeField] private Button FillButton;
    [SerializeField] private Button EmptyButton;
    [SerializeField] private Button MoveButton;
    [SerializeField] private Button ExitButton;

    [SerializeField] private BucketManager BucketManager;

    private void Awake()
    {
        HowButton.onClick.AddListener(() => OpenHowToPlay());
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
    private void OpenHowToPlay()
    {

    }
    private void ExitWaterGame()
    {

    }
}
