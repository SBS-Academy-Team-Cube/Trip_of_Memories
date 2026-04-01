[System.Serializable]
public class SaveData
{
    public int Version = 1;
    public bool HasPlayed = false;
    public bool CanEnterLevelSelect = false;
    // Progress
    public int StageIndex;

    // Player Information
    public float PlayerHp;
    public float[] PlayerPosition;

    // Character Select
    public int SelectedCharacterModelIndex;
}