[System.Serializable]
public class SaveData
{
    public int Version = 1;
    public bool HasPlayed;

    // Progress
    public int StageIndex;

    // Player Information
    public float PlayerHp;
    public float[] PlayerPosition;

    // Character Select
    public int SelectedCharacterModelIndex;
}