using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] CharacterPrefabs;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private CinemachineCamera CinemachineCamera;
    [SerializeField] private SpringArm CameraArm;
    [SerializeField] private CutsceneManager CutScene;
    [SerializeField] private WorldUIManager WorldUI;
    [SerializeField] private Transform PlayerCameraPivot;
    public System.Action<GameObject> OnPlayerSpawned;
    void Start()
    {
        if (SaveManager.Instance.Data == null)
        {
            SaveManager.Instance.Load();
        }
        int index = SaveManager.Instance.Data.SelectedCharacterModelIndex;
        if (index < 0 || index >= CharacterPrefabs.Length)
        {
            Debug.LogError("Wrong Character Index");
            return;
        }
        GameObject Player = Instantiate(CharacterPrefabs[index], transform.position, transform.rotation);
        OnPlayerSpawned?.Invoke(Player);

        if (Player.TryGetComponent(out PlayerMovement Move))
        {
            PlayerCameraPivot = Move.CameraPivot;
            Move.CameraTransform = MainCamera.transform;
            CinemachineCamera.Target.TrackingTarget = Move.CameraPivot;
            CameraArm.SetTarget(Move.CameraPivot);

            if (CutScene != null)
            {
                CutScene.SetPlayerMovement(Move);
            }
        }
        if (WorldUI != null && Player.TryGetComponent(out PlayerInteraction Interaction))
        {
            WorldUI.SetPlayerInteraction(Interaction);
        }
        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.FadeIn(MainCamera.WorldToViewportPoint(PlayerCameraPivot.position));
        }
    }
}
