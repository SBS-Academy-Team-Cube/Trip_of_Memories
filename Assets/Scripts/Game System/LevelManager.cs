using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string LevelId;
    [SerializeField] private SceneId FirstSceneId;
    [SerializeField] private SceneId NextSceneId;
    [SerializeField] private Portal NextPortal;
    [Header("Audio Settings")]
    [SerializeField] private AudioClip LevelBGM;
    [SerializeField] private bool bKeepBGMPlaybackForNextScene;
    void OnEnable()
    {
        if (NextPortal != null)
        {
            NextPortal.OnTriggered += LoadNextScene;
        }
        if (SaveManager.Instance && SaveManager.Instance.CurrentLevelProgress == null)
        {
            SaveManager.Instance.BeginLevel(LevelId, FirstSceneId);
        }
    }
    void OnDisable()
    {
        if (NextPortal != null)
        {
            NextPortal.OnTriggered -= LoadNextScene;
        }
    }
    private void Start()
    {
        if (AudioManager.Instance && LevelBGM)
        {
            AudioManager.Instance.PlayBGM(LevelBGM);
        }
    }
    private void LoadNextScene()
    {
        if (GameDirector.Instance == null || GameDirector.Instance.Iris == null)
        {
            Debug.Log("Error with Load Next Scene in Level Manager...");
            return;
        }
        StartCoroutine(NextSceneLoadRoutine());
    }
    private IEnumerator NextSceneLoadRoutine()
    {
        float IrisFadeDuration = GameDirector.Instance.Iris.FadeOut();
        float BGMFadeDuration = 0.0f;
        if (AudioManager.Instance)
        {
            BGMFadeDuration = AudioManager.Instance.StopBGM(bKeepBGMPlaybackForNextScene);
        }

        yield return new WaitForSeconds(Mathf.Max(IrisFadeDuration, BGMFadeDuration) + 0.1f);
        GameDirector.Instance.LoadSceneWithoutLoading(NextSceneId);
    }
}
