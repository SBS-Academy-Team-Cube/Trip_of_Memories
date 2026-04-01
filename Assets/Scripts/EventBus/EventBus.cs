using System;
using UnityEngine;

public static class EventBus
{
    public static event Action<GameObject> LeverOn;

    public static event Action<GameObject> LeverOff;

    public static void PublishLeverOn(GameObject lever)
    {
        LeverOn?.Invoke(lever);
    }
    public static void PublishLeverOff(GameObject lever)
    {
        LeverOff?.Invoke(lever);
    }
}
