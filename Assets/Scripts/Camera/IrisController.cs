using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class IrisController : MonoBehaviour
{
    [SerializeField] private float FadeDuration = 0.75f;
    private float InitialRadius = 1.5f;
    [SerializeField] private Image Image;
    private Material Mat;
    public Action<bool> OnFadeInTransition;
    private void Awake()
    {
        Mat = Instantiate(Image.material);
        Image.material = Mat;
        Mat.SetVector("_Center", new Vector2(0.5f, 0.5f));
    }
    public void FadeIn()
    {
        if (Mat)
        {
            Mat.SetFloat("_Radius", 0.0f);
            StartTransition(true);
        }
    }
    public void FadeOut()
    {
        if (Mat)
        {
            Mat.SetFloat("_Radius", 1.2f);
            StartTransition(false);
        }
    }
    public void StartTransition(bool IsFadeIn)
    {
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
        if (!IsFadeIn)
        {
            Mat.SetFloat("_Smoothness", 0.0f);
        }
        OnFadeInTransition?.Invoke(IsFadeIn);
    }
}
