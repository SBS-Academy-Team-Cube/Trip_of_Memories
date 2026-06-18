using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionRestricter : MonoBehaviour
{
    [SerializeField] private List<InputActionReference> BanedActions;

    float Timer = 0.0f;
    void OnEnable()
    {
        foreach (InputActionReference Action in BanedActions)
        {
            Action.action.Disable();
        }
    }
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 2.5f)
        {
            foreach (InputActionReference Action in BanedActions)
            {
                Action.action.Disable();
            }
            Timer = -10000f;
        }
    }
    void OnDisable()
    {
        foreach (InputActionReference Action in BanedActions)
        {
            Action.action.Enable();
        }
    }
}
