[System.Serializable]
public class SaveData
{
    public int Version = 1;
    public bool HasPlayed = false;
    public bool CanEnterLevelSelect = false;

    // Progress
    public int StageIndex;

    // Character Select
    public int SelectedCharacterModelIndex;
}