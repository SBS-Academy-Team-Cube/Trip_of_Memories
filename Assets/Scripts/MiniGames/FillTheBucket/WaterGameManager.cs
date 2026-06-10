using UnityEngine;
using System;
public class WaterGameManager : MiniGameBase
{
    public override event Action OnPlay;
    public override event Action OnClear;
    public override event Action OnFail;
    public string ID => MiniGameID;
    [SerializeField] private BucketManager BucketManager;
    [SerializeField] private BucketUIManager UI;
    [SerializeField] private GameObject Canvas;

    public override bool HasCleared()
    {
        if (SaveManager.Instance != null)
        {
            return SaveManager.Instance.HasClearedMiniGame(MiniGameID);
        }
        return false;
    }
    public override void Play()
    {
        if (GameDirector.Instance)
        {
            GameDirector.Instance.ShowMouseCursor(true);
            GameDirector.Instance.EnablePauseAction(false);
        }
        Canvas.SetActive(true);
        BucketManager.Init();
        OnPlay?.Invoke();
    }
    public override void Clear()
    {
        if (GameDirector.Instance)
        {
            GameDirector.Instance.ShowMouseCursor(false);
            GameDirector.Instance.EnablePauseAction(true);
        }
        if (SaveManager.Instance)
        {
            SaveManager.Instance.TryClearMiniGame(MiniGameID, MemoryRecoveryAmount);
        }
        Canvas.SetActive(false);
        OnClear?.Invoke();
    }
    public override void Fail()
    {
        if (GameDirector.Instance)
        {
            GameDirector.Instance.ShowMouseCursor(false);
            GameDirector.Instance.EnablePauseAction(true);
        }
        BucketManager.Reset();
        Canvas.SetActive(false);
        OnFail?.Invoke();
    }
}
