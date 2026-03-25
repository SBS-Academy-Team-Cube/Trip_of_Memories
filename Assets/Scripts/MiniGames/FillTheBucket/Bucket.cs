using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Bucket : MonoBehaviour,IBucket
{
    [Header("Water Image")]
    [SerializeField] private Image waterFillImage;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI Text;

    [Header("Setting")]
    [SerializeField] private float maxCapacity = 3f;     
    [SerializeField] private bool canBeFilled = true;     // 3L is true
    [SerializeField] private bool isOverflowSensitive = false; // 5L is true

    private float currentWater = 0f;

    public UnityEvent FailEvent = new UnityEvent();
    public UnityEvent ClearEvent = new UnityEvent();

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
        // If water overflows in the 5L bucket, trigger failure event
        if (isOverflowSensitive && (currentWater + amount > maxCapacity))
        {
            FailEvent.Invoke();
            return 0f;
        }
        float oldAmount = currentWater;
        currentWater = Mathf.Clamp(currentWater + amount, 0f, maxCapacity);
        float actuallyAdded = currentWater - oldAmount;

        UpdateFill();
        // If the 5L bucket is exactly full with water, trigger clear event
        if (isOverflowSensitive && currentWater == maxCapacity)
            ClearEvent.Invoke();
        return actuallyAdded;   // Return the actual distance moved
    }

    private void UpdateFill() // Update image, text
    {
        if (waterFillImage != null)
        {
            waterFillImage.fillAmount = currentWater / maxCapacity;
        }
        if(Text != null)
        {
            Text.text = currentWater.ToString() + " L";
        }
    }
}
