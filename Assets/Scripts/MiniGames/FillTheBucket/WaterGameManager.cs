using UnityEngine;
using UnityEngine.Events;
using System;
public class WaterGameManager : MonoBehaviour
{
    [SerializeField] private string UniqueID;
    public string ID => UniqueID;
    [SerializeField] private BucketManager BucketManager;
    [SerializeField] private BucketUIManager UI;
    [SerializeField] private GameObject Canvas;
    public UnityEvent OnPlay;
    public event Action OnClear;
    public event Action OnFail;
    public void Play()
    {
        if (GameDirector.Instance)
        {
            GameDirector.Instance.ShowMouseCursor(true);
        }
        Canvas.SetActive(true);
        BucketManager.Init();
        OnPlay?.Invoke();
    }
    public void Clear()
    {
        if (GameDirector.Instance)
        {
            GameDirector.Instance.ShowMouseCursor(false);
        }
        Canvas.SetActive(false);
        OnClear?.Invoke();
    }
    public void Fail()
    {
        if (GameDirector.Instance)
        {
            GameDirector.Instance.ShowMouseCursor(false);
        }
        OnFail?.Invoke();
    }
}
