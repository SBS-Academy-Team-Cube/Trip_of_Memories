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

    private void OnEnable()
    {
        if (SaveManager.Instance)
        {
            SaveManager.Instance.OnDisplayedMemoryRecoveryChanged += UpdateFillAmount;
        }
    }
    private void OnDisable()
    {
        if (SaveManager.Instance)
        {
            SaveManager.Instance.OnDisplayedMemoryRecoveryChanged -= UpdateFillAmount;
        }
    }
    private void Start()
    {
        if (SaveManager.Instance)
        {
            UpdateFillAmount(SaveManager.Instance.GetDisplayedMemoryRecoveryPercent());
        }
    }
    public void UpdateFillAmount(int Amount)
    {
        StartCoroutine(UpdateRoutine(Amount / 100.0f));
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