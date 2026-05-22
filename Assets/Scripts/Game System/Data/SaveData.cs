using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int Version = 1;
    public bool HasPlayed = false;
    public bool CanEnterLevelSelect = false;

    // Progress
    public int StageIndex;

    // Character Select
    public int SelectedCharacterModelIndex;

    // Global memory recovery progress. Keep this in the 0~100 range.
    public int MemoryRecoveryPercent;

    // Permanent reward/completion records. These prevent duplicate rewards on replay.
    public List<string> ClearedLevelIds = new List<string>();
    public List<string> CollectedMemoryItemIds = new List<string>();
    public List<string> ClearedMiniGameIds = new List<string>();
    public List<string> CompletedInteractionIds = new List<string>();

    public void Normalize()
    {
        if (ClearedLevelIds == null)
        {
            ClearedLevelIds = new List<string>();
        }

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

        MemoryRecoveryPercent = ClampPercent(MemoryRecoveryPercent);
    }

    public bool HasClearedLevel(string levelId)
    {
        return ContainsId(ClearedLevelIds, levelId);
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
    public bool AddClearedLevel(string levelId)
    {
        return AddUniqueId(ClearedLevelIds, levelId);
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
        MemoryRecoveryPercent = ClampPercent(MemoryRecoveryPercent + Math.Max(0, amount));
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

    private static int ClampPercent(int value)
    {
        return Math.Max(0, Math.Min(100, value));
    }
}
