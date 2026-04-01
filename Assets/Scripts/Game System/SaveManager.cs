using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string Path;
    public SaveData Data { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        Path = Application.persistentDataPath + "/save.json";
    }

    public void Load()
    {
        if(!File.Exists(Path))
        {
            Debug.Log("No Save File -> New Creation");
            Data = CreateNewData();
            Save();
            return;
        }
        string Json = File.ReadAllText(Path);
        Data = JsonUtility.FromJson<SaveData>(Json);
        if (Data == null)
        {
            Debug.LogWarning("Corrupted save data → Reset");
            Data = CreateNewData();
            Save();
        }
    }
    
    public void Save()
    {
        string Json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(Path, Json);
        Debug.Log(Application.persistentDataPath);
        Debug.Log("Save Complete");
    }

    private SaveData CreateNewData()
    {
        return new SaveData
        {
            HasPlayed = false,
            StageIndex = 0,
            SelectedCharacterModelIndex = -1,
        };
    }
    private void DeleteSave()
    {
        if (File.Exists(Path))
        {
            File.Delete(Path);
            Debug.Log("Save file deleted");
        }
        Data = CreateNewData();
    }
    public void ResetSave()
    {
        DeleteSave();
        Save();
    }
}
