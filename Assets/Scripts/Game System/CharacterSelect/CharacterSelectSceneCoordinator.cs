using UnityEngine;

public class CharacterSelectSceneCoordinator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private StoryImagePresenter Presenter;
    [SerializeField] private CharacterSelectController selectController;
    [SerializeField] private SceneId NextSceneID;

    [SerializeField] private AudioClip BGM;

    [Header("Story Data")]
    [SerializeField] private StoryData SceneStroyData;

    [Header("UI")]
    [SerializeField] private GameObject storyUIRoot;
    [SerializeField] private GameObject selectUIRoot;


    private void OnEnable()
    {
        if (storyManager != null)
        {
            Presenter.OnFadeToBlackComplete += EnterSelectState;
        }
        if (selectController != null)
        {
            selectController.OnCharacterConfirmed += HandleCharacterConfirmed;
        }
    }
    private void OnDisable()
    {
        if (storyManager != null)
        {
            Presenter.OnFadeToBlackComplete -= EnterSelectState;
        }
        if (selectController != null)
        {
            selectController.OnCharacterConfirmed -= HandleCharacterConfirmed;
        }
    }
    private void Start()
    {
        EnterStoryState();
        if (AudioManager.Instance && BGM)
        {
            AudioManager.Instance.PlayBGM(BGM);
        }
    }

    void EnterStoryState()
    {
        storyUIRoot.SetActive(true);
        selectUIRoot.SetActive(false);

        storyManager.enabled = true;
        storyManager.ShowStory(SceneStroyData);
        selectController.enabled = false;
    }
    void EnterSelectState()
    {
        storyUIRoot.SetActive(false);
        selectUIRoot.SetActive(true);

        storyManager.enabled = false;
        selectController.enabled = true;
    }
    void HandleCharacterConfirmed(int characterIndex)
    {
        if (SaveManager.Instance)
        {
            SaveManager.Instance.CompleteSelection(characterIndex);
        }
        if (AudioManager.Instance)
        {
            AudioManager.Instance.StopBGM();
        }
        if (GameDirector.Instance)
        {
            GameDirector.Instance.LoadScene(NextSceneID);
        }
    }
}