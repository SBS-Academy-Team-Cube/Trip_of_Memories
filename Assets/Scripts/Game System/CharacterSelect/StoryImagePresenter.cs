using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoryImagePresenter : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private DialoguePresentationData presentationData;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image FadeInOutImage;
    public System.Action OnFadeToBlackComplete;
    [Header("Fade Setting")]
    [SerializeField] private float fadeDuration = 0.5f;
    private Coroutine fadeCoroutine;

    private void OnEnable()
    {
        storyManager.OnStoryChanged += HandleStoryChanged;
        storyManager.OnStoryEnd += HandleDialougeEnd;
    }

    private void OnDisable()
    {
        storyManager.OnStoryChanged -= HandleStoryChanged;
        storyManager.OnStoryEnd -= HandleDialougeEnd;
    }
    void HandleStoryChanged(FStoryContext Context, int Index)
    {
        if (presentationData == null)
        {
            return;
        }
        Sprite nextSprite = presentationData.GetBackground(Index);
        if (nextSprite == null)
        {
            return;
        }
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeRoutine(nextSprite));
    }
    void HandleDialougeEnd()
    {
        StartCoroutine(EndFadeRoutine());
    }
    IEnumerator EndFadeRoutine()
    {
        float time = 0f;
        Color color = backgroundImage.color;
        float fadeInOutDuration = 0.25f;
        while (time < fadeInOutDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeInOutDuration;

            color.a = Mathf.Lerp(1f, 0f, t);
            backgroundImage.color = color;

            yield return null;
        }
        time = 0f;
        color = FadeInOutImage.color;
        while (time < fadeInOutDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeInOutDuration;

            color.a = Mathf.Lerp(1f, 0f, t);
            FadeInOutImage.color = color;
            yield return null;
        }
        OnFadeToBlackComplete?.Invoke();
    }
    IEnumerator FadeRoutine(Sprite nextSprite)
    {
        float time = 0f;
        Color color = backgroundImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            color.a = Mathf.Lerp(1f, 0f, t);
            backgroundImage.color = color;

            yield return null;
        }
        backgroundImage.sprite = nextSprite;
        if (nextSprite == null)
        {
            backgroundImage.gameObject.SetActive(false);
            yield break;
        }
        else
        {
            backgroundImage.gameObject.SetActive(true);
        }
        time = 0f;
        color.a = 0f;
        backgroundImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            color.a = Mathf.Lerp(0f, 1f, t);
            backgroundImage.color = color;

            yield return null;
        }
    }


}