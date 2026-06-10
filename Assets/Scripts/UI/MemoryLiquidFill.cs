using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MemoryLiquidFill : MonoBehaviour
{
    private static readonly int FillAmountId = Shader.PropertyToID("_FillAmount");
    [SerializeField] private Image TargetImage;
    [SerializeField] private float Duration = 1.0f;

    private Material RuntimeMaterial;
    private Coroutine UpdateCoroutine;

    private void Awake()
    {
        if (!TargetImage)
        {
            TargetImage = GetComponent<Image>();
        }

        RuntimeMaterial = Instantiate(TargetImage.material);
        TargetImage.material = RuntimeMaterial;
    }

    private void OnValidate()
    {
        if (!TargetImage)
        {
            TargetImage = GetComponent<Image>();
        }
    }

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

        if (UpdateCoroutine != null)
        {
            StopCoroutine(UpdateCoroutine);
            UpdateCoroutine = null;
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
        if (UpdateCoroutine != null)
        {
            StopCoroutine(UpdateCoroutine);
        }

        UpdateCoroutine = StartCoroutine(UpdateRoutine(Amount / 100.0f));
    }
    private IEnumerator UpdateRoutine(float Target)
    {
        if (Duration <= 0.0f)
        {
            RuntimeMaterial.SetFloat(FillAmountId, Target);
            TargetImage.SetMaterialDirty();
            UpdateCoroutine = null;
            yield break;
        }

        float Timer = 0.0f;
        float Start = RuntimeMaterial.GetFloat(FillAmountId);
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            RuntimeMaterial.SetFloat(FillAmountId, Mathf.SmoothStep(Start, Target, Timer / Duration));
            TargetImage.SetMaterialDirty();
            yield return null;
        }
        RuntimeMaterial.SetFloat(FillAmountId, Target);
        TargetImage.SetMaterialDirty();
        UpdateCoroutine = null;
    }

    private void OnDestroy()
    {
        if (RuntimeMaterial)
        {
            Destroy(RuntimeMaterial);
        }
    }
}
