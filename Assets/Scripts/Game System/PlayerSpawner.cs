using UnityEngine;
using System;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] CharacterPrefabs;
    [SerializeField] private CameraManager CameraManager;
    [SerializeField] private CutsceneManager CutSceneManager;
    [SerializeField] private UIHPController HPUI;
    private bool IsRespawning = false;
    private Health PlayerHP;
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
                IsRespawning = true;
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
        CutSceneManager?.Init(Player);
        if (CameraManager != null)
        {
            CameraManager.Init(Player);
        }

        if (Player.TryGetComponent(out Health PlayerHealth))
        {
            InitPlayerHealth(PlayerHealth);
            HPUI?.Init(PlayerHealth);
        }
    }

    private void InitPlayerHealth(Health PlayerHealth)
    {
        if (PlayerHP != null)
        {
            PlayerHP.OnHPChanged -= OnPlayerHPChanged;
            PlayerHP.OnDead -= ReSpawn;
        }

        PlayerHP = PlayerHealth;

        int InitialHealth = IsRespawning
            ? PlayerHP.MaxHealth
            : SaveManager.Instance.GetCurrentPlayerHealthOrDefault(PlayerHP.MaxHealth);

        PlayerHP.Init(InitialHealth);
        SaveManager.Instance.SetCurrentPlayerHealth(PlayerHP.HP);
        PlayerHP.OnHPChanged += OnPlayerHPChanged;
        PlayerHP.OnDead += ReSpawn;
    }

    private void OnDestroy()
    {
        if (PlayerHP != null)
        {
            PlayerHP.OnHPChanged -= OnPlayerHPChanged;
            PlayerHP.OnDead -= ReSpawn;
        }
    }

    private void OnPlayerHPChanged(int CurrentHP)
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SetCurrentPlayerHealth(CurrentHP);
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
        if (GameDirector.Instance == null || GameDirector.Instance.Iris == null || SaveManager.Instance == null || SaveManager.Instance.CurrentLevelProgress == null)
        {
            Debug.Log("Error in Respawn...");
            return;
        }
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(GameDirector.Instance.Iris.FadeOut() + 0.1f);
        var ProgressData = SaveManager.Instance.CurrentLevelProgress;
        GameDirector.Instance.LoadScene(ProgressData.HasCheckpoint() ? ProgressData.LastCheckpoint.SceneID : ProgressData.FirstSceneId);
    }
}
