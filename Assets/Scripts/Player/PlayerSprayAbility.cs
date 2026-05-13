using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSprayAbility : MonoBehaviour
{
    [SerializeField] private PlayerAnimation Animation;
    [SerializeField] private SprayItem Spray;
    [SerializeField] private float CoolDownDelay = 2.0f;
    bool IsHolding = false;
    bool CanUse = false;
    bool IsUsing = false;
    public void TryTakeSpray()
    {
        if (IsUsing)
        {
            return;
        }
        IsHolding = !IsHolding;
        CanUse = IsHolding;
        Animation.SetTakeSpray(IsHolding);
    }
    public void TryUse()
    {
        if (IsHolding && CanUse)
        {
            StartCoroutine(UseRoutine());
        }
    }
    private IEnumerator UseRoutine()
    {
        IsUsing = true;

        Animation.SetInterpolatedLayerWeight(ETargetLayer.RightArm, 1.0f, 0.35f);
        yield return new WaitForSeconds(0.35f);
        
        Spray.Shoot();
        yield return new WaitForSeconds(Spray.GetDuration() + 0.15f);
        StartCoroutine(CoolDown());

        Animation.SetInterpolatedLayerWeight(ETargetLayer.RightArm, 0.15f, 0.35f);
        yield return new WaitForSeconds(0.35f);

        IsUsing = false;
    }
    private IEnumerator CoolDown()
    {
        CanUse = false;
        yield return new WaitForSeconds(CoolDownDelay);
        CanUse = true;
    }
}
