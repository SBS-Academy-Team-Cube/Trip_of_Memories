using UnityEngine;
using UnityEngine.UI;

public class Bucket : MonoBehaviour,IBucket
{
    [Header("Water Image")]
    [SerializeField] private Image waterFillImage;

    [Header("Setting")]
    [SerializeField] private float maxCapacity = 3f;     
    [SerializeField] private bool canBeFilled = true;     // 3L is true

    private float currentWater = 0f;

    public float MaxCapacity() => maxCapacity;
    public float CurrentWater() => currentWater;
    public bool CanBeFilled => canBeFilled;

    public void Init()
    {
        currentWater = 0f;
        UpdateFill();
    }

    public void SetWaterAmount(float amount)
    {
        currentWater = Mathf.Clamp(amount, 0f, maxCapacity);
        UpdateFill();
    }

    public float AddWater(float amount)
    {
        float oldAmount = currentWater;
        currentWater = Mathf.Clamp(currentWater + amount, 0f, maxCapacity);
        float actuallyAdded = currentWater - oldAmount;

        UpdateFill();
        return actuallyAdded;   // Return the actual distance moved
    }

    private void UpdateFill() // Update image
    {
        if (waterFillImage != null)
        {
            waterFillImage.fillAmount = currentWater / maxCapacity;
        }
    }
}
