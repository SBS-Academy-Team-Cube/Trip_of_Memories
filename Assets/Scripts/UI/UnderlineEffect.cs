using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UnderlineEffect : MonoBehaviour
{
    [SerializeField] private Image UnderlineImage;
    [SerializeField] private Image PencilImage;
    [SerializeField] private RectTransform UnderlineRect;
    [SerializeField] private RectTransform PencilRect;
    [SerializeField] private AudioSource Audio;
    private Coroutine PlayingEffect = null;
    public float EndPosX = 170.0f;
    public float Duration = 1.0f;

    private void Awake()
    {
        Audio.ignoreListenerPause = true;
    }
    public void Play(float TargetPosY)
    {
        UnderlineRect.anchoredPosition = new Vector2(UnderlineRect.anchoredPosition.x, TargetPosY);
        UnderlineImage.enabled = true;
        PencilImage.enabled = true;
        PlayingEffect = StartCoroutine(Effect());
        if (Audio != null && Audio.clip != null)
        {
            Audio.Play();
        }
    }
    public void Stop()
    {
        if (PlayingEffect != null)
        {
            StopCoroutine(PlayingEffect);
            PlayingEffect = null;
        }
        UnderlineImage.enabled = false;
        PencilImage.enabled = false;
        SetProgress(0.0f);
    }
    private IEnumerator Effect()
    {
        float Elapsed = 0.0f;
        while (Elapsed <= Duration)
        {
            Elapsed += Time.unscaledDeltaTime;
            float t = Elapsed / Duration;
            float eased = t * t * (3f - 2f * t);
            SetProgress(eased);
            yield return null;
        }
        SetProgress(1.0f);
        PlayingEffect = null;
    }
    private void SetProgress(float T)
    {
        UnderlineImage.fillAmount = T;
        PencilRect.anchoredPosition = new Vector2(EndPosX * T, 0.0f);
    }
}
