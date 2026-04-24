using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class IrisController : MonoBehaviour
{
    [SerializeField] private float FadeDuration = 0.75f;
    private float InitialRadius = 1.2f;

    [SerializeField] private SpriteRenderer Render;
    private Material Mat;
    [SerializeField] private Camera mainCamera;
    
    public UnityEvent<bool> OnTransitionEnd;

    private void Awake() 
    {
        Mat = Instantiate(Render.material);
        Render.material = Mat;
    }    
    public void StartTransition(Vector3 TargetPosition, bool IsFadeIn)
    {
        if(mainCamera && Mat)
        {
            Vector3 screenPos = mainCamera.WorldToViewportPoint(TargetPosition);
            Mat.SetVector("_Center", new Vector2(screenPos.x, screenPos.y));
            StartCoroutine(TransitionRoutine(IsFadeIn));
        }
    }
    private IEnumerator TransitionRoutine(bool IsFadeIn)
    {
        float Timer = 0f;
        float StartRadius = IsFadeIn ? 0f : InitialRadius;
        float EndRadius = StartRadius + (IsFadeIn ? InitialRadius : -InitialRadius);
        while(Timer < FadeDuration)
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
