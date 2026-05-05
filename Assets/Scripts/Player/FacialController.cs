using System.Collections;
using UnityEngine;

public class FacialController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer TargetRenderer;
    public float Duration = 1.0f;
    private float Timer = 0.0f;
    private float EyeBlinkingRandomInterval = 5.0f;
    private void SetBlendShape(int Idx, float Weight)
    {
        if (!TargetRenderer)
        {
            return;
        }
        TargetRenderer.SetBlendShapeWeight(Idx, Mathf.Clamp(Weight, 0f, 100f));
    }

    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= EyeBlinkingRandomInterval)
        {
            Timer = 0.0f;
            EyeBlinkingRandomInterval = Random.Range(8.0f, 12.0f);
            StartCoroutine(BlinkingRoutine());
        }
    }
    private IEnumerator BlinkingRoutine()
    {
        float Timer = 0.0f;
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            SetBlendShape(0, Mathf.Lerp(0f, 100.0f, Timer / Duration));
            yield return null;
        }
        SetBlendShape(0, 100.0f);
        yield return new WaitForSeconds(0.1f);
        Timer = 0.0f;
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            SetBlendShape(0, Mathf.Lerp(100.0f, 0.0f, Timer / Duration));
            yield return null;
        }
        SetBlendShape(0, 0.0f);
    }
}
