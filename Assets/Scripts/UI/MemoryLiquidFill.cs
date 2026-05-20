using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class MemoryLiquidFill : MonoBehaviour
{
    private static readonly int FillAmountId = Shader.PropertyToID("_FillAmount");

    [SerializeField] private Image TargetImage;
    [SerializeField] private float Duration = 1.0f;
    public void UpdateFillAmount(float Amount)
    {
        StartCoroutine(UpdateRoutine(Amount));
    }
    private IEnumerator UpdateRoutine(float Target)
    {
        float Timer = 0.0f;
        float Start = TargetImage.material.GetFloat(FillAmountId);
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            TargetImage.material.SetFloat(FillAmountId, Mathf.SmoothStep(Start, Target, Timer / Duration));
            TargetImage.SetMaterialDirty();
            yield return null;
        }
        TargetImage.material.SetFloat(FillAmountId, Target);
        TargetImage.SetMaterialDirty();
    }
}