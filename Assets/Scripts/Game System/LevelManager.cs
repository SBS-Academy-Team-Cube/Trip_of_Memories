using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class LevelManager : MonoBehaviour
{
    public List<UnityEvent> LevelEvents;
    private int DoorConditionIndex = 0;
    [SerializeField] private int RequiredConditionCounts = 2;
    [SerializeField] private SceneId NextSceneId;

    [SerializeField] private IrisController TransitionController;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip LevelBGM;
    void OnEnable()
    {
        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.OnFadeInTransition += OnFadeOutEnd;
        }
    }
    void Osable()
    {
        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.OnFadeInTransition -= OnFadeOutEnd;
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
    public void OnConditionMet()
    {
        DoorConditionIndex++;
        if (DoorConditionIndex == RequiredConditionCounts)
        {
            LevelEvents[0]?.Invoke();
        }
    }
    public void CallTransition(Vector3 ViewPortPosition)
    {
        if (GameDirector.Instance && GameDirector.Instance.Iris)
        {
            GameDirector.Instance.Iris.FadeOut(ViewPortPosition);
        }
    }
}
