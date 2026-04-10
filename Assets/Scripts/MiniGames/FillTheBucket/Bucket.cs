using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Bucket : MonoBehaviour, IBucket
{
    [Header("Sprite")]
    [SerializeField] private Image TargetImage;
    [SerializeField] private Sprite FilledSprite;
    [SerializeField] private Sprite FullFilledSprite;
    [SerializeField] private Sprite EmptySprite;
    [SerializeField] private Sprite PouringSprite;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI Text;

    [Header("Setting")]
    [SerializeField] private int maxCapacity = 3;     
    [SerializeField] private bool canBeFilled = true;     // 3L is true
    public bool CanBeFilled => canBeFilled;
    [SerializeField] private bool isOverflowSensitive = false; // 5L is true

    private int currentWater = 0;
    
    public UnityEvent FailEvent = new UnityEvent();
    public UnityEvent ClearEvent = new UnityEvent();

    public int MaxCapacity() => maxCapacity;
    public int CurrentWater() => currentWater;
    

    public void Init()
    {
        currentWater = 0;
        UpdateFill();
    }

    public bool CanFill()
    {
        return currentWater != maxCapacity; 
    }

    // public void Fill(out int OutAmount)
    // {
    //     int AcceptableAmount = OutAmount - (maxCapacity - currentWater);
    //     currentWater += AcceptableAmount;
    //     UpdateFill();
    //     OutAmount -= AcceptableAmount;
    // }

    public void SetWaterAmount(float amount)
    {
        // currentWater = Math.Clamp(amount, 0, maxCapacity);
        // UpdateFill();
    }

    public void OnSelected(int MyIndex)
    {
        
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
        // currentWater = Mathf.Clamp(currentWater + amount, 0f, maxCapacity);
        float actuallyAdded = currentWater - oldAmount;

        UpdateFill();
        // If the 5L bucket is exactly full with water, trigger clear event
        if (isOverflowSensitive && currentWater == maxCapacity)
            ClearEvent.Invoke();
        return actuallyAdded;   // Return the actual distance moved
    }

    private void UpdateFill() // Update image, text
    {
        if (TargetImage != null)
        {
            TargetImage.fillAmount = currentWater / maxCapacity;
        }
        if(Text != null)
        {
            Text.text = currentWater.ToString() + " L";
        }
    }
}
