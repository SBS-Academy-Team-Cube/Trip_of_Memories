using UnityEngine;

public interface IBucket
{
    int MaxCapacity();
    int CurrentWater();
    void SetWaterAmount(float amount);
    float AddWater(float amount);
    void Init();
    bool CanBeFilled { get; } 
}
