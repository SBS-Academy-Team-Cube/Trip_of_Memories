using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSprayAbility : MonoBehaviour
{
    [SerializeField] private PlayerAnimation Animation;
    [SerializeField] private SprayItem Spray;
    [SerializeField] private PlayerState State;
    [SerializeField] private float CoolDownDelay = 2.0f;
    [SerializeField] private float ArmUpDelay, ArmDownDelay, WaitOffset;
    private bool IsHolding = false;
    private bool CanUse = false;
    public bool IsUsing = false;
    public void TryTakeSpray()
    {
        if (State.IsInteracting || IsUsing || !State.IsGrounded || State.InterAction != PlayerState.EInterAction.None)
        {
            return;
        }
        IsHolding = !IsHolding;
        CanUse = IsHolding;
        State.SetAbility(PlayerState.EAbility.Spray);
        Animation.SetTakeSpray(IsHolding);
    }
    public void TryUse()
    {
        if (IsUsing || !CanUse || !State.IsGrounded)
        {
            return;
        }
        StartCoroutine(UseRoutine());
    }
    private IEnumerator UseRoutine()
    {
        IsUsing = true;
        State.IsInteracting = true;
        Animation.SetIsMoving(false);

        Animation.SetInterpolatedLayerWeight(ETargetLayer.RightArm, 1.0f, ArmUpDelay);
        yield return new WaitForSeconds(ArmUpDelay);

        Spray.Shoot(transform.rotation);
        yield return new WaitForSeconds(Spray.GetDuration() + WaitOffset);
        StartCoroutine(CoolDown());

        Animation.SetInterpolatedLayerWeight(ETargetLayer.RightArm, 0.15f, ArmDownDelay);
        yield return new WaitForSeconds(ArmDownDelay);

        State.IsInteracting = false;
        IsUsing = false;
    }
    private IEnumerator CoolDown()
    {
        CanUse = false;
        yield return new WaitForSeconds(CoolDownDelay);
        CanUse = true;
    }
}
