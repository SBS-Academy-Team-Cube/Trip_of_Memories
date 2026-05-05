using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private Slider ProgressBar;
    [SerializeField] private TMP_Text TipText;
    [SerializeField] private string[] RandomTips;
    [SerializeField] private float MinLoadTime = 2.0f;

    void Start()
    {
        SetRandomTip();
        StartCoroutine(LoadSceneAsync());
    }
    void SetRandomTip()
    {
        if (RandomTips.Length == 0)
        {
            return;
        }
        TipText.text = RandomTips[Random.Range(0, RandomTips.Length)];
    }
    IEnumerator LoadSceneAsync()
    {
        if (GameDirector.Instance == null)
        {
            Debug.Log("Can't Find GameDirector Instance");
            yield break;
        }
        AsyncOperation Operation;


        Operation = GameDirector.Instance.AsyncLoading();
        Operation.allowSceneActivation = false;

        float Timer = 0f;

        while (!Operation.isDone)
        {
            Timer += Time.deltaTime;

            float Progress = Mathf.Clamp01(Operation.progress / 0.9f);
            float TimeProgress = Mathf.Clamp01(Timer / MinLoadTime);

            float FinalProgress = Mathf.Min(Progress, TimeProgress);

            ProgressBar.value = FinalProgress;

            if (Progress >= 1f && Timer >= MinLoadTime)
            {
                Operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}