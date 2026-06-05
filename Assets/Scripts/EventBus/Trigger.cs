using UnityEngine;
using System;

public class Trigger : MonoBehaviour
{
    public event Action OnTriggered;
    
    public void OnTrigger()
    {
        OnTriggered?.Invoke();
    }
}
