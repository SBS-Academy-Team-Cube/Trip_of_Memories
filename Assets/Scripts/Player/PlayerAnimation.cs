using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private const string LeftArmLayerName = "Left Arm Layer";
    private const string RightArmLayerName = "Right Arm Layer";
    private const string HeadLayerName = "Head Layer";
    private const string BodyLayerName = "Body Layer";
    [SerializeField] private Animator AnimationController;
    private int hangingHash;
    private int LeftArmLayerIndex = -1;
    private int RightArmLayerIndex = -1;
    private int HeadLayerIndex = -1;
    private int BodyLayerIndex = -1;
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

        LeftArmLayerIndex = GetLayerIndex(LeftArmLayerName, "Left Arm");
        RightArmLayerIndex = GetLayerIndex(RightArmLayerName, "Right Arm");
        HeadLayerIndex = AnimationController.GetLayerIndex(HeadLayerName);
        BodyLayerIndex = AnimationController.GetLayerIndex(BodyLayerName);

    }
    private int GetLayerIndex(params string[] layerNames)
    {
        foreach (string layerName in layerNames)
        {
            int layerIndex = AnimationController.GetLayerIndex(layerName);
            if (layerIndex >= 0)
            {
                return layerIndex;
            }
        }

        return -1;
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
        SetLayerWeight(LeftArmLayerIndex, Weight);
        SetLayerWeight(RightArmLayerIndex, Weight);
        SetLayerWeight(BodyLayerIndex, Weight);
        SetLayerWeight(HeadLayerIndex, Weight);
    }
    private void SetLayerWeight(int layerIndex, float weight)
    {
        if (layerIndex < 0)
        {
            return;
        }
        AnimationController.SetLayerWeight(layerIndex, Mathf.Clamp01(weight));
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
        AnimationController.SetBool("IsPushing", bPushing);
        AnimationController.SetTrigger("LeverPushTrigger");
        SetLeverPlaying(0.0f);
    }
    public void SetLeverPlaying(float Speed)
    {
        AnimationController.SetFloat("LeverAnimSpeed", Speed);
    }
    public void IsPlay(bool bPlaying)
    {
        AnimationController.speed = bPlaying ? 1.0f : 0.0f;
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
        AnimationController.SetTrigger(IsHanging ? "StartHangingTrigger" : "EndHangingTrigger");
        if (IsHanging)
        {
            SetRopePlaying(0.0f);
        }
    }
    public void SetRopePlaying(float Speed)
    {
        AnimationController.SetFloat("RopeAnimSpeed", Speed);
    }
    public void PlayRopeAnimation(bool bReverse)
    {
        AnimatorStateInfo info = AnimationController.GetCurrentAnimatorStateInfo(0);
        float time = info.normalizedTime;
        time += bReverse ? -1 : 1 * Time.deltaTime;
        if (time > 1f || time < 0f)
        {
            time += bReverse ? 1f : -1f;
        }
        AnimationController.Play(info.fullPathHash, 0, time);
        AnimationController.speed = 0f;
    }
    public void PlayForward()
    {
        AnimatorStateInfo info = AnimationController.GetCurrentAnimatorStateInfo(0);
        if (info.fullPathHash != hangingHash)
        {
            return;
        }
        float time = info.normalizedTime;
        time += Time.deltaTime;

        if (time > 1f)
            time -= 1f;

        AnimationController.Play(info.fullPathHash, 0, time);
        AnimationController.speed = 0f;
    }
}
