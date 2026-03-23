using UnityEngine;

public interface IBucket
{
    float MaxCapacity();
    float CurrentWater();
    void SetWaterAmount(float amount);
    float AddWater(float amount);
    void Init();
    bool CanBeFilled { get; } 
}
