using UnityEngine;
public class LevelSelectSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] CharacterPrefabs;
    [SerializeField] private CameraManager CameraManager;
    [SerializeField] private LevelPortal[] LevelPortals;

    private void OnEnable()
    {
        if (SaveManager.Instance != null)
        {
            if (SaveManager.Instance.Data == null)
            {
                SaveManager.Instance.Load();
            }
            int ClearedLevel = SaveManager.Instance.Data.ClearedLevelIds.Count;
            for (int i = 0; i < LevelPortals.Length; i++)
            {
                LevelPortals[i].Init(i == ClearedLevel, i < ClearedLevel);
            }
        }
    }
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
        CameraManager.Init(PlayerInstance);

        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.FadeIn();
        }
    }

}
