using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnderlineEffect : MonoBehaviour
{
    [SerializeField] private Canvas RootCanvas;
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
        CacheRootCanvas();
        if (Audio)
        {
            Audio.ignoreListenerPause = true;
        }
    }
    public void Play(RectTransform TargetRect)
    {
        MoveUnderlineToCanvasY(GetCanvasBottomY(TargetRect));
        PlayEffect();
    }
    public void Play(float TargetCanvasPosY)
    {
        MoveUnderlineToCanvasY(TargetCanvasPosY);
        PlayEffect();
    }
    private void PlayEffect()
    {
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
    private void MoveUnderlineToCanvasY(float TargetCanvasPosY)
    {
        RectTransform CanvasRect = GetRootCanvasRect();
        if (CanvasRect == null || UnderlineRect == null)
        {
            return;
        }

        Vector3 CanvasPosition = CanvasRect.InverseTransformPoint(UnderlineRect.position);
        CanvasPosition.y = TargetCanvasPosY;
        UnderlineRect.position = CanvasRect.TransformPoint(CanvasPosition);
    }
    private float GetCanvasBottomY(RectTransform TargetRect)
    {
        RectTransform CanvasRect = GetRootCanvasRect();
        if (CanvasRect == null || TargetRect == null)
        {
            return UnderlineRect != null ? UnderlineRect.anchoredPosition.y : 0.0f;
        }

        Bounds TargetBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(CanvasRect, TargetRect);
        return TargetBounds.min.y;
    }
    private RectTransform GetRootCanvasRect()
    {
        CacheRootCanvas();
        return RootCanvas != null ? RootCanvas.transform as RectTransform : null;
    }
    private void CacheRootCanvas()
    {
        if (RootCanvas != null)
        {
            RootCanvas = RootCanvas.rootCanvas;
            return;
        }

        if (UnderlineRect != null)
        {
            RootCanvas = UnderlineRect.GetComponentInParent<Canvas>()?.rootCanvas;
        }

        if (RootCanvas == null)
        {
            RootCanvas = GetComponentInParent<Canvas>()?.rootCanvas;
        }
    }
}
