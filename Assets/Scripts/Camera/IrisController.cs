using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;

public class IrisController : MonoBehaviour
{
    [SerializeField] private float FadeDuration = 0.75f;
    private float InitialRadius = 1.5f;
    [SerializeField] private Image Image;
    private Material Mat;
    public UnityEvent<bool> OnTransitionEnd;

    private void Awake()
    {
        Mat = Instantiate(Image.material);
        Image.material = Mat;
    }
    public void FadeIn(Vector3 ViewPortPosition)
    {
        if (Mat)
        {
            Mat.SetFloat("_Radius", 0.0f);
            StartTransition(ViewPortPosition, true);
        }
    }
    public void FadeOut(Vector3 ViewPortPosition)
    {
        if (Mat)
        {
            Mat.SetFloat("_Radius", 1.2f);
            StartTransition(ViewPortPosition, false);
        }
    }
    public void StartTransition(Vector3 ViewPortPosition, bool IsFadeIn)
    {
        Mat.SetVector("_Center", new Vector2(ViewPortPosition.x, ViewPortPosition.y));
        StartCoroutine(TransitionRoutine(IsFadeIn));
    }
    private IEnumerator TransitionRoutine(bool IsFadeIn)
    {
        float Timer = 0f;
        float StartRadius = Mat.GetFloat("_Radius");
        float EndRadius = StartRadius + (IsFadeIn ? InitialRadius : -InitialRadius);
        while (Timer < FadeDuration)
        {
            Timer += Time.deltaTime;
            float t = Timer / FadeDuration;

            Mat.SetFloat("_Radius", Mathf.Lerp(StartRadius, EndRadius, t));
            yield return null;
        }
        Mat.SetFloat("_Radius", EndRadius);
        OnTransitionEnd?.Invoke(IsFadeIn);
    }
}
