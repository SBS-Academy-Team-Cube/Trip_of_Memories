using UnityEngine;
using UnityEngine.UI;

public class WaterGameManager : MonoBehaviour
{
    [SerializeField] private BucketManager BucketManager;
    [SerializeField] private BucketUIManager UI;
    [SerializeField] private GameObject Canvas;
    private void Start()
    {
        Play();
    }
    public void Play()
    {
        Canvas.SetActive(true);
        BucketManager.Init();
    }
    public void Clear()
    {

    }
    public void Fail()
    {

    }
}
