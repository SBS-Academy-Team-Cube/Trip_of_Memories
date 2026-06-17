using UnityEngine;
using System;
using UnityEngine.Events;
public class Trigger : MonoBehaviour
{
    public event Action OnTriggered;
    public UnityEvent WhenTriggered;
    public void OnTrigger()
    {
        OnTriggered?.Invoke();
        WhenTriggered?.Invoke();
    }
}
