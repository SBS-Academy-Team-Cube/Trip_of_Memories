using System;

public static class EventBus
{
    public static event Action PentominoClear;
    public static void PublishPentominoClear()
    {
        PentominoClear?.Invoke();
    }
}
