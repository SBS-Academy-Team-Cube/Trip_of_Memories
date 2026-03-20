using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform PlayerStart;
    [SerializeField] private GameObject[] CharacterPrefabs;
    public System.Action<GameObject> OnPlayerSpawned;
    [SerializeField] private Transform CameraTransform;
    void Start()
    {
        if(SaveManager.Instance.Data == null)
        {
            SaveManager.Instance.Load();
        }
        int index = SaveManager.Instance.Data.SelectedCharacterModelIndex;
        if (index < 0 || index >= CharacterPrefabs.Length)
        {
            Debug.LogError("Wrong Character Index");
            return;
        }
        GameObject Player = Instantiate(CharacterPrefabs[index], PlayerStart.position, PlayerStart.rotation);
        if (Player.TryGetComponent(out PlayerMovement Move))
        {
            Move.CameraTransform = CameraTransform;
        }
        OnPlayerSpawned?.Invoke(Player);
    }
}
