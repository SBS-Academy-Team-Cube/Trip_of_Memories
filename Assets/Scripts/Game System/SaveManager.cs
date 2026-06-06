using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string Path;
    public SaveData Data { get; private set; }
    public LevelProgressData CurrentLevelProgress { get; private set; }
    public event System.Action<int> OnDisplayedMemoryRecoveryChanged;
    protected override void Awake()
    {
        base.Awake();
        Path = Application.persistentDataPath + "/save.json";
    }

    public void Load()
    {
        if (!File.Exists(Path))
        {
            Debug.Log("No Save File -> New Creation");
            Data = CreateNewData();
            Save();
            return;
        }

        string json = File.ReadAllText(Path);
        Data = JsonUtility.FromJson<SaveData>(json);
        if (Data == null)
        {
            Debug.LogWarning("Corrupted save data ??Reset");
            Data = CreateNewData();
            Save();
            return;
        }

        Data.Normalize();
    }

    public void Save()
    {
        if (Data == null)
        {
            Data = CreateNewData();
        }

        Data.Normalize();

        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(Path, json);
        Debug.Log(Application.persistentDataPath);
        Debug.Log("Save Complete");
    }

    public void BeginLevel(string levelId, SceneId FirstSceneId)
    {
        CurrentLevelProgress = new LevelProgressData(levelId, FirstSceneId);
    }
    public void ClearCurrentLevelProgress()
    {
        CurrentLevelProgress = null;
    }
    public bool HasActiveLevelProgress()
    {
        return CurrentLevelProgress != null;
    }

    private void NotifyDisplayedMemoryRecoveryChanged()
    {
        OnDisplayedMemoryRecoveryChanged?.Invoke(GetDisplayedMemoryRecoveryPercent());
    }
    public int GetDisplayedMemoryRecoveryPercent()
    {
        EnsureData();
        int currentLevelAmount = CurrentLevelProgress != null
            ? CurrentLevelProgress.SessionMemoryRecoveryPercent
            : 0;

        return Mathf.Clamp(Data.MemoryRecoveryPercent + currentLevelAmount, 0, 100);
    }

    public bool TryCollectMemoryItem(string memoryItemId, int memoryRecoveryAmount)
    {
        EnsureLevelProgress();

        if (Data.HasCollectedMemoryItem(memoryItemId) || CurrentLevelProgress.HasCollectedMemoryItem(memoryItemId))
        {
            return false;
        }

        CurrentLevelProgress.AddCollectedMemoryItem(memoryItemId);
        CurrentLevelProgress.AddMemoryRecovery(memoryRecoveryAmount);
        NotifyDisplayedMemoryRecoveryChanged();
        return true;
    }
    public bool TryClearMiniGame(string miniGameId, int memoryRecoveryAmount)
    {
        EnsureLevelProgress();

        if (Data.HasClearedMiniGame(miniGameId) || CurrentLevelProgress.HasClearedMiniGame(miniGameId))
        {
            return false;
        }
        CurrentLevelProgress.AddClearedMiniGame(miniGameId);
        CurrentLevelProgress.AddMemoryRecovery(memoryRecoveryAmount);
        NotifyDisplayedMemoryRecoveryChanged();
        return true;
    }

    public bool TryCompleteInteraction(string interactionId, int memoryRecoveryAmount = 0)
    {
        EnsureLevelProgress();

        if (Data.HasCompletedInteraction(interactionId) || CurrentLevelProgress.HasCompletedInteraction(interactionId))
        {
            return false;
        }

        CurrentLevelProgress.AddCompletedInteraction(interactionId);
        CurrentLevelProgress.AddMemoryRecovery(memoryRecoveryAmount);
        NotifyDisplayedMemoryRecoveryChanged();
        return true;
    }
    public void SetCheckpoint(SceneId SceneID, string checkpointId)
    {
        EnsureLevelProgress();
        CurrentLevelProgress.SetCheckpoint(SceneID, checkpointId);
    }

    public void SetCurrentPlayerHealth(int playerHealth)
    {
        EnsureLevelProgress();
        CurrentLevelProgress.PlayerHealth = Mathf.Max(0, playerHealth);
    }

    public int GetCurrentPlayerHealthOrDefault(int defaultHealth)
    {
        EnsureLevelProgress();

        if (CurrentLevelProgress.PlayerHealth <= 0)
        {
            return defaultHealth;
        }

        return Mathf.Clamp(CurrentLevelProgress.PlayerHealth, 1, defaultHealth);
    }

    public void CompleteCurrentLevel()
    {
        EnsureLevelProgress();

        Data.AddMemoryRecovery(CurrentLevelProgress.SessionMemoryRecoveryPercent);
        Data.AddClearedLevel(CurrentLevelProgress.LevelId);

        foreach (string memoryItemId in CurrentLevelProgress.CollectedMemoryItemIds)
        {
            Data.AddCollectedMemoryItem(memoryItemId);
        }

        foreach (string miniGameId in CurrentLevelProgress.ClearedMiniGameIds)
        {
            Data.AddClearedMiniGame(miniGameId);
        }

        foreach (string interactionId in CurrentLevelProgress.CompletedInteractionIds)
        {
            Data.AddCompletedInteraction(interactionId);
        }

        CurrentLevelProgress = null;
        Save();
    }

    public void ResetSave()
    {
        DeleteSave();
        Save();
    }

    private SaveData CreateNewData()
    {
        SaveData data = new SaveData
        {
            HasPlayed = false,
            StageIndex = 0,
            SelectedCharacterModelIndex = -1,
        };

        data.Normalize();
        return data;
    }

    private void DeleteSave()
    {
        if (File.Exists(Path))
        {
            File.Delete(Path);
            Debug.Log("Save file deleted");
        }

        Data = CreateNewData();
        CurrentLevelProgress = null;
    }

    private void EnsureData()
    {
        if (Data == null)
        {
            Load();
        }
        Data.Normalize();
    }

    private void EnsureLevelProgress()
    {
        EnsureData();
        if (CurrentLevelProgress == null)
        {
            Debug.LogWarning("No active level progress. Creating an unnamed level progress session.");
            CurrentLevelProgress = new LevelProgressData("unknown_level", SceneId.NULL);
        }

        CurrentLevelProgress.Normalize();
    }
}
