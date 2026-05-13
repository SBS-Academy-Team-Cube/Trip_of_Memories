using System.Collections;
using System.IO.Compression;
using System.Transactions;
using UnityEngine;
using UnityEngine.EventSystems;


public enum ETargetLayer { LeftArm, RightArm, Head, Body };
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
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
        // LeftArmLayerIndex = AnimationControllerGetLayerIndex(LeftArmLayerName, "Left Arm");
        // RightArmLayerIndex = GetLayerIndex(RightArmLayerName, "Right Arm");
        // HeadLayerIndex = AnimationController.GetLayerIndex(HeadLayerName);
        // BodyLayerIndex = AnimationController.GetLayerIndex(BodyLayerName);

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
        AnimationController.SetBool("IsMoving", IsMoving);
    }
    
    public void SetGait(int Gait)
    {
        AnimationController.SetInteger("Gait", Gait);
    }
    
    public void SetPickup()
    {
        AnimationController.SetBool("IsHolding", true);
        AnimationController.SetTrigger("PickingTrigger");
    }
    public void SetPickDown()
    {
        AnimationController.SetBool("IsHolding", false);
        AnimationController.SetTrigger("PickingTrigger");
    }
    public void SetLeverPush(bool bPushing)
    {
        AnimationController.SetTrigger(bPushing ? "StartLeverPushTrigger" : "EndLeverPushTrigger");
        SetLeverPlaying(0.0f);
    }
    public void SetLeverPlaying(float Speed)
    {
        AnimationController.SetFloat("LeverAnimSpeed", Speed);
    }
    
    public void SetSpeed(float Speed)
    {
        AnimationController.SetFloat("Speed", Speed);
    }
    public void SetJump()
    {
        AnimationController.SetTrigger("Jump");
    }
    public void SetMantling()
    {
        AnimationController.applyRootMotion = true;
        AnimationController.SetTrigger("Mantling");
    }
    public void DisableRootMotion()
    {
        AnimationController.applyRootMotion = false;
    }
    public void SetHangOnRope(bool IsHanging)
    {
        AnimationController.SetBool("IsHanging", IsHanging);
        // AnimationController.SetTrigger(IsHanging ? "StartHangingTrigger" : "EndHangingTrigger");
        if (IsHanging)
        {
            SetRopePlaying(0.0f);
        }
    }
    public void SetRopePlaying(float Speed)
    {
        AnimationController.SetFloat("RopeAnimSpeed", Speed);
    }

    public void SetTakeSpray(bool bHolding)
    {
        AnimationController.SetTrigger("TakeSprayTrigger");
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
        Debug.Log("Animation Event Called");
        SetInterpolatedLayerWeight(ETargetLayer.RightArm, 0.15f, 0.25f);
    }
    public void OnTakeInSpray()
    {
        Debug.Log("On Take In Animation Event Called");
        SetInterpolatedLayerWeight(ETargetLayer.RightArm, 0.0f, 0.5f);
    }
}
