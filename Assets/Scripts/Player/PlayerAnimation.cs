using System.Collections;
using System.IO.Compression;
using System.Transactions;
using UnityEngine;
using UnityEngine.EventSystems;


public enum ETargetLayer { LeftArm, RightArm, Head, Body };
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerState State;
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int GaitHash = Animator.StringToHash("Gait");
    private static readonly int IsHoldingHash = Animator.StringToHash("IsHolding");
    private static readonly int PickingTriggerHash = Animator.StringToHash("PickingTrigger");
    private static readonly int LeverPushTriggerHash = Animator.StringToHash("LeverPushTrigger");
    private static readonly int LeverAnimSpeedHash = Animator.StringToHash("LeverAnimSpeed");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int MantlingHash = Animator.StringToHash("Mantling");
    private static readonly int IsHangingHash = Animator.StringToHash("IsHanging");
    private static readonly int RopeAnimSpeedHash = Animator.StringToHash("RopeAnimSpeed");
    private static readonly int TakeSprayTriggerHash = Animator.StringToHash("TakeSprayTrigger");
    private static readonly string[] LayerNames =
    { "Left Arm Layer", "Right Arm Layer", "Head Layer", "Body Layer" };
    private int[] LayerIndexs = { -1, -1, -1, -1 };
    private Coroutine[] InterpolatedLayers = { null, null, null, null };
    [SerializeField] private Animator AnimationController;
    private Transform leftHandIKTarget;
    private Transform rightHandIKTarget;
    private float leftHandIKWeight;
    private float rightHandIKWeight;

    private void Awake()
    {
        if (!AnimationController)
        {
            AnimationController = GetComponent<Animator>();
        }
        for (int i = 0; i < LayerNames.Length; i++)
        {
            LayerIndexs[i] = AnimationController.GetLayerIndex(LayerNames[i]);
        }
    }
    void Update()
    {
        if (State)
        {
            AnimationController.SetBool(IsGroundedHash, State.IsGrounded);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        ApplyHandIK(AvatarIKGoal.LeftHand, leftHandIKTarget, leftHandIKWeight);
        ApplyHandIK(AvatarIKGoal.RightHand, rightHandIKTarget, rightHandIKWeight);
        Vector3 pos = AnimationController.GetIKPosition(AvatarIKGoal.RightHand);
        Quaternion quat = AnimationController.GetIKRotation(AvatarIKGoal.RightHand);

        float length = 1.0f;
        Debug.DrawLine(pos, pos + quat * Vector3.forward * length, Color.blue);
        Debug.DrawLine(pos, pos + quat * Vector3.up * length, Color.green);
        Debug.DrawLine(pos, pos + quat * Vector3.right * length, Color.red);
    }
    public void SetHandIKTargets(Transform leftTarget, Transform rightTarget)
    {
        leftHandIKTarget = leftTarget;
        rightHandIKTarget = rightTarget;
    }
    public void SetHandIKWeight(float leftWeight, float rightWeight)
    {
        leftHandIKWeight = Mathf.Clamp01(leftWeight);
        rightHandIKWeight = Mathf.Clamp01(rightWeight);
    }
    public void ClearHandIK()
    {
        leftHandIKTarget = null;
        rightHandIKTarget = null;
        leftHandIKWeight = 0.0f;
        rightHandIKWeight = 0.0f;
    }
    private void ApplyHandIK(AvatarIKGoal goal, Transform target, float weight)
    {
        if (!target)
        {
            AnimationController.SetIKPositionWeight(goal, 0.0f);
            AnimationController.SetIKRotationWeight(goal, 0.0f);
            return;
        }
        AnimationController.SetIKPositionWeight(goal, weight);
        AnimationController.SetIKRotationWeight(goal, weight);
        AnimationController.SetIKPosition(goal, target.position);
        AnimationController.SetIKRotation(goal, target.rotation);
    }
    public void EnableHoldingLayer(bool bEnable)
    {
        float Weight = bEnable ? 1.0f : 0.0f;
        SetLayerWeight(ETargetLayer.LeftArm, Weight);
        SetLayerWeight(ETargetLayer.RightArm, Weight);
        SetLayerWeight(ETargetLayer.Head, Weight);
        SetLayerWeight(ETargetLayer.Body, Weight);
    }
    public void SetLayerWeight(ETargetLayer Layer, float Weight)
    {
        if ((int)Layer < 0)
        {
            return;
        }
        AnimationController.SetLayerWeight(LayerIndexs[(int)Layer], Mathf.Clamp01(Weight));
    }
    public void SetIsMoving(bool IsMoving)
    {
        AnimationController.SetBool(IsMovingHash, IsMoving);
    }
    public void SetGait(int Gait)
    {
        AnimationController.SetInteger(GaitHash, Gait);
    }
    public void SetIsHolding(bool IsHolding)
    {
        AnimationController.SetBool(IsHoldingHash, IsHolding);
        AnimationController.SetTrigger(PickingTriggerHash);
    }
    public void SetLeverPush(bool bPushing)
    {
        // AnimationController.SetTrigger(bPushing ? "StartLeverPushTrigger" : "EndLeverPushTrigger");
        AnimationController.SetTrigger(LeverPushTriggerHash);
        SetLeverPlaying(0.0f);
    }
    public void SetLeverPlaying(float Speed)
    {
        AnimationController.SetFloat(LeverAnimSpeedHash, Speed);
    }
    public void SetSpeed(float Speed)
    {
        AnimationController.SetFloat(SpeedHash, Speed);
    }
    public void SetJump()
    {
        AnimationController.SetTrigger(JumpHash);
    }
    public void SetMantling()
    {
        AnimationController.applyRootMotion = true;
        AnimationController.SetTrigger(MantlingHash);
    }
    public void DisableRootMotion()
    {
        AnimationController.applyRootMotion = false;
    }
    public void SetHangOnRope(bool IsHanging)
    {
        AnimationController.SetBool(IsHangingHash, IsHanging);
        if (IsHanging)
        {
            SetRopePlaying(0.0f);
        }
    }
    public void SetRopePlaying(float Speed)
    {
        AnimationController.SetFloat(RopeAnimSpeedHash, Speed);
    }
    public void SetTakeSpray(bool bHolding)
    {
        AnimationController.SetTrigger(TakeSprayTriggerHash);
        SetInterpolatedLayerWeight(ETargetLayer.RightArm, 1.0f, 0.15f);
    }
    public void SetInterpolatedLayerWeight(ETargetLayer Layer, float TargetWeight, float Duration)
    {
        if (InterpolatedLayers[(int)Layer] != null)
        {
            StopCoroutine(InterpolatedLayers[(int)Layer]);
        }
        InterpolatedLayers[(int)Layer] = StartCoroutine(InterpolateLayerWeightRoutine(Layer, TargetWeight, Duration));
    }
    private IEnumerator InterpolateLayerWeightRoutine(ETargetLayer Layer, float TargetWeight, float Duration)
    {
        float StartWeight = AnimationController.GetLayerWeight(LayerIndexs[(int)Layer]);
        float Timer = 0.0f;
        while (Timer < Duration)
        {
            Timer += Time.deltaTime;
            SetLayerWeight(Layer, Mathf.SmoothStep(StartWeight, TargetWeight, Timer / Duration));
            yield return null;
        }
        SetLayerWeight(Layer, TargetWeight);
        InterpolatedLayers[(int)Layer] = null;
    }
    public void OnTakeOutSpray()
    {
        SetInterpolatedLayerWeight(ETargetLayer.RightArm, 0.15f, 0.25f);
    }
    public void OnTakeInSpray()
    {
        SetInterpolatedLayerWeight(ETargetLayer.RightArm, 0.0f, 0.5f);
    }
}
