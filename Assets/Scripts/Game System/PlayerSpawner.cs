using UnityEngine;
using Unity.Cinemachine;
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
        int Index = SaveManager.Instance.Data.SelectedCharacterModelIndex;
        if (Index < 0 || Index >= CharacterPrefabs.Length)
        {
            return;
        }
        GameObject Player = Instantiate(CharacterPrefabs[Index], transform.position, transform.rotation);
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
            GameDirector.Instance.Iris.FadeIn(new Vector3(0.5f, 0.5f, 0.0f));
        }
    }
}
