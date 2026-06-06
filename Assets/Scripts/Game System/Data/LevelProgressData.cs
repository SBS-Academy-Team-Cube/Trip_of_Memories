using System;
using System.Collections.Generic;

[Serializable]
public class LevelProgressData
{
    public string LevelId;

    public int SessionMemoryRecoveryPercent;
    public int PlayerHealth;

    // Temporary records for the current level run.
    public List<string> CollectedMemoryItemIds = new List<string>();
    public List<string> ClearedMiniGameIds = new List<string>();
    public List<string> CompletedInteractionIds = new List<string>();

    public CheckpointData LastCheckpoint;
    public SceneId FirstSceneId;

    public LevelProgressData()
    {

    }

    public LevelProgressData(string levelId, SceneId FirstScene)
    {
        LevelId = levelId;
        FirstSceneId = FirstScene;
        Normalize();
    }

    public void Normalize()
    {
        if (CollectedMemoryItemIds == null)
        {
            CollectedMemoryItemIds = new List<string>();
        }

        if (ClearedMiniGameIds == null)
        {
            ClearedMiniGameIds = new List<string>();
        }

        if (CompletedInteractionIds == null)
        {
            CompletedInteractionIds = new List<string>();
        }
        SessionMemoryRecoveryPercent = Math.Max(0, SessionMemoryRecoveryPercent);
        PlayerHealth = Math.Max(0, PlayerHealth);
    }

    public bool HasCheckpoint()
    {
        return LastCheckpoint != null;
    }

    public bool HasCollectedMemoryItem(string memoryItemId)
    {
        return ContainsId(CollectedMemoryItemIds, memoryItemId);
    }

    public bool HasClearedMiniGame(string miniGameId)
    {
        return ContainsId(ClearedMiniGameIds, miniGameId);
    }
    public bool HasCompletedInteraction(string interactionId)
    {
        return ContainsId(CompletedInteractionIds, interactionId);
    }
    public bool AddCollectedMemoryItem(string memoryItemId)
    {
        return AddUniqueId(CollectedMemoryItemIds, memoryItemId);
    }

    public bool AddClearedMiniGame(string miniGameId)
    {
        return AddUniqueId(ClearedMiniGameIds, miniGameId);
    }

    public bool AddCompletedInteraction(string interactionId)
    {
        return AddUniqueId(CompletedInteractionIds, interactionId);
    }

    public void AddMemoryRecovery(int amount)
    {
        SessionMemoryRecoveryPercent += Math.Max(0, amount);
    }

    public void SetCheckpoint(SceneId SceneID, string checkpointId)
    {
        LastCheckpoint = new CheckpointData(SceneID, checkpointId);
    }

    private static bool ContainsId(List<string> ids, string id)
    {
        return !string.IsNullOrWhiteSpace(id) && ids != null && ids.Contains(id);
    }

    private static bool AddUniqueId(List<string> ids, string id)
    {
        if (ids == null || string.IsNullOrWhiteSpace(id) || ids.Contains(id))
        {
            return false;
        }

        ids.Add(id);
        return true;
    }
}

[Serializable]
public class CheckpointData
{
    public SceneId SceneID;
    public string CheckpointID;
    public CheckpointData()
    {
    }
    public CheckpointData(SceneId SceneID, string CheckpointID)
    {
        this.SceneID = SceneID;
        this.CheckpointID = CheckpointID;
    }
}
