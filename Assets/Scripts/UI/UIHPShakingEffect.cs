using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIHPShakingEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform Target;
    [SerializeField] private Image TargetImage;

    [Header("Shake")]
    [SerializeField] private float ShakeDuration = 1.0f;
    [SerializeField] private float MaxAngle = 15.0f;
    [SerializeField] private float Frequency = 6.0f;

    [Header("Color Fade")]
    [SerializeField] private float ColorFadeDuration = 0.35f;
    private Color DisabledColor = new Color32(0x60, 0x60, 0x60, 180);
    private Color OriginalColor;
    private Coroutine CurrentRoutine;
    private void Awake()
    {
        OriginalColor = TargetImage.color;
    }
    public void Init(bool bEnable)
    {
        TargetImage.color = bEnable ? OriginalColor : DisabledColor;
    }
    public void Reset()
    {
        TargetImage.color = OriginalColor;
    }
    public void Play()
    {
        if (CurrentRoutine != null)
        {
            StopCoroutine(CurrentRoutine);
        }
        CurrentRoutine = StartCoroutine(PlayRoutine());
    }
    private IEnumerator PlayRoutine()
    {
        yield return ShakeRoutine();

        yield return ColorFadeRoutine();

        CurrentRoutine = null;
    }
    private IEnumerator ShakeRoutine()
    {
        float Timer = 0.0f;
        while (Timer < ShakeDuration)
        {
            Timer += Time.deltaTime;
            float Normalized = Timer / ShakeDuration;
            float Damping = Mathf.Pow(1.0f - Normalized, 2.0f);
            float Angle = Mathf.Sin(Normalized * Mathf.PI * 2.0f * Frequency) * MaxAngle * Damping;
            Target.localRotation = Quaternion.Euler(0.0f, 0.0f, Angle);
            yield return null;
        }
        Target.localRotation = Quaternion.identity;
    }
    private IEnumerator ColorFadeRoutine()
    {
        Color StartColor = TargetImage.color;
        float Timer = 0.0f;
        while (Timer < ColorFadeDuration)
        {
            Timer += Time.deltaTime;
            float Normalized = Timer / ColorFadeDuration;
            TargetImage.color = Color.Lerp(StartColor, DisabledColor, Mathf.SmoothStep(0.0f, 1.0f, Normalized));
            yield return null;
        }
        TargetImage.color = DisabledColor;
    }
}
