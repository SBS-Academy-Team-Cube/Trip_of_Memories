using UnityEngine;
using Unity.Cinemachine;
using System;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] CharacterPrefabs;
    [SerializeField] private CameraManager CameraManager;
    [SerializeField] private CutsceneManager CutSceneManager;
    public Action<GameObject> OnPlayerSpawned;
    [SerializeField] private UIHPController HPUI;
    void Start()
    {
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        var progress = SaveManager.Instance.CurrentLevelProgress;
        if (progress != null && progress.HasCheckpoint() && GameDirector.Instance.CurrentSceneID == progress.LastCheckpoint.SceneID)
        {
            CheckPoint checkpoint = FindCheckpoint(progress.LastCheckpoint.CheckpointID);
            if (checkpoint != null)
            {
                spawnPosition = checkpoint.SpawnPosition.position;
                spawnRotation = checkpoint.SpawnPosition.rotation;
            }
        }
        

        if (SaveManager.Instance.Data == null)
        {
            SaveManager.Instance.Load();
        }
        int Index = SaveManager.Instance.Data.SelectedCharacterModelIndex;
        if (Index < 0 || Index >= CharacterPrefabs.Length)
        {
            return;
        }
        GameObject PlayerInstance = Instantiate(CharacterPrefabs[Index], spawnPosition, spawnRotation);
        OnPlayerSpawned?.Invoke(PlayerInstance);
        Initialize(PlayerInstance);

        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.FadeIn();
        }
    }
    public void Initialize(GameObject Player)
    {
        if (Player.TryGetComponent(out PlayerMovement Movement))
        {
            Movement.SetCameraTransform(CameraManager?.GetCameraTransform());
            
        }
        CutSceneManager?.SetPlayerMovement(Player);
        if (CameraManager != null)
        {
            CameraManager.Init(Player);
        }

        if (HPUI != null && Player.TryGetComponent(out Health PlayerHP))
        {
            HPUI.Init(PlayerHP);
            PlayerHP.OnDead += ReSpawn;
        }
    }
    private CheckPoint FindCheckpoint(string checkpointId)
    {
        CheckPoint[] checkpoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);

        foreach (CheckPoint checkpoint in checkpoints)
        {
            if (checkpoint.CheckpointID == checkpointId)
            {
                return checkpoint;
            }
        }
        return null;
    }

    public void ReSpawn()
    {
        if(GameDirector.Instance != null && GameDirector.Instance.Iris != null)
        {
            GameDirector.Instance.Iris.FadeOut();
        }
        
        if(SaveManager.Instance == null || GameDirector.Instance == null)
        {
            return;
        }

        var ProgressData = SaveManager.Instance.CurrentLevelProgress;
        if(ProgressData != null)
        {            
            GameDirector.Instance.LoadScene(ProgressData.HasCheckpoint() ? ProgressData.LastCheckpoint.SceneID : ProgressData.FirstSceneId);
        }
    
    }
}
