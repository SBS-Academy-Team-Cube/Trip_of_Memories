using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string LevelId;
    [SerializeField] private SceneId FirstSceneId;
    [SerializeField] private SceneId NextSceneId;
    [SerializeField] private Portal NextPortal;
    [Header("Audio Settings")]
    [SerializeField] private AudioClip LevelBGM;
    void OnEnable()
    {
        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.OnFadeInTransition += OnFadeOutEnd;
        }
        if (NextPortal != null)
        {
            NextPortal.OnTriggered += CallTransition;
        }
        if (SaveManager.Instance && SaveManager.Instance.CurrentLevelProgress == null)
        {
            SaveManager.Instance.BeginLevel(LevelId, FirstSceneId);
        }
    }
    void OnDisable()
    {
        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.OnFadeInTransition -= OnFadeOutEnd;
        }
        if (NextPortal != null)
        {
            NextPortal.OnTriggered -= CallTransition;
        }
    }
    private void Start()
    {
        if (AudioManager.Instance && LevelBGM)
        {
            AudioManager.Instance.PlayBGM(LevelBGM);
        }
    }
    private void OnFadeOutEnd(bool IsFadeIn)
    {
        if (IsFadeIn)
        {
            return;
        }
        if (AudioManager.Instance)
        {
            AudioManager.Instance.StopBGM();
        }
        if (GameDirector.Instance)
        {

            GameDirector.Instance.LoadSceneWithoutLoading(NextSceneId);
        }
    }
    public void CallTransition()
    {
        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.FadeOut();
        }
    }
}
