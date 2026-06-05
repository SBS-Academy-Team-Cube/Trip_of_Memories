using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private CheckpointData Data;

    public Transform SpawnPosition;

    public string CheckpointID => Data.CheckpointID;
    public SceneId SceneID => Data.SceneID;

    private bool bEnabled = false;
    
    private void OnEnable()
    {

    }
    private void OnTriggerEnter(Collider Other)
    {
        if (!Other.CompareTag("Player")) 
        { 
            return;
        }
        SaveManager.Instance.SetCheckpoint(Data.SceneID, Data.CheckpointID);
    }
}
