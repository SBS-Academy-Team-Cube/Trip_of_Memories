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
        if (SaveManager.Instance.Data == null)
        {
            SaveManager.Instance.Load();
        }
        int Index = SaveManager.Instance.Data.SelectedCharacterModelIndex;
        if (Index < 0 || Index >= CharacterPrefabs.Length)
        {
            return;
        }
        GameObject PlayerInstance = Instantiate(CharacterPrefabs[Index], transform.position, transform.rotation);
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
            CutSceneManager?.SetPlayerMovement(Movement);
        }

        if (CameraManager != null)
        {
            CameraManager.Init(Player);
        }

        if (HPUI != null && Player.TryGetComponent(out Health PlayerHP))
        {
            HPUI.Init(PlayerHP);
        }
    }
}
