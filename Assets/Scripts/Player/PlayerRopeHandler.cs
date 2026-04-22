using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRopeHandler : MonoBehaviour
{
    [SerializeField] private PlayerState State;
    [SerializeField] private Animator animator;

    private Transform TargetRope;
    private Vector3 LeftHandTarget;
    private Vector3 RightHandTarget;
    private Quaternion LeftHandRot;
    private Quaternion RightHandRot;
    private float HandIKWeight;
    private bool bCaptureHand = false;
    public UnityEvent<bool> OnEnterHanging;
    public float Speed;
    public bool bIsMoving { get; private set; }
    public void GrapRope(GameObject Target)
    {
        float clampedY;
        if (Target.TryGetComponent(out Renderer renderer))
        {
            Bounds bounds = renderer.bounds;
            clampedY = Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y);
            Debug.Log(clampedY);
        }
        else
        {
            return;
        }
        TargetRope = Target.transform;
        OnEnterHanging?.Invoke(true);
        if (State)
        {
            State.SetAction(PlayerState.EAction.Hanging);
        }
        if (TryGetComponent(out CharacterController cc))
        {
            cc.enabled = false;
            transform.position = new Vector3(Target.transform.position.x, clampedY, Target.transform.position.z);
            cc.enabled = true;
        }
    }
    public void ReleaseRope()
    {
        if (State)
        {
            State.SetAction(PlayerState.EAction.None);
        }
        OnEnterHanging?.Invoke(false);
    }
    public void CanMove()
    {
        bIsMoving = true;
        bCaptureHand = true;
        HandIKWeight = 1.0f;
    }
    public void WaitForNextMove()
    {
        bIsMoving = false;
        HandIKWeight = 0.0f;
    }
    // void OnAnimatorIK(int layerIndex)
    // {
    //     if (bCaptureHand)
    //     {
    //         LeftHandTarget = TargetRope.InverseTransformPoint(animator.GetIKPosition(AvatarIKGoal.LeftHand));
    //         RightHandTarget = TargetRope.InverseTransformPoint(animator.GetIKPosition(AvatarIKGoal.RightHand));

    //         LeftHandRot = animator.GetIKRotation(AvatarIKGoal.LeftHand);
    //         RightHandRot = animator.GetIKRotation(AvatarIKGoal.RightHand);
    //         bCaptureHand = false;
    //     }
    //     animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKWeight);
    //     animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKWeight);

    //     animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKWeight);
    //     animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandIKWeight);

    //     if (TargetRope)
    //     {
    //         Debug.Log(TargetRope.TransformPoint(LeftHandTarget));
    //         animator.SetIKPosition(AvatarIKGoal.LeftHand, TargetRope.TransformPoint(LeftHandTarget));
    //         animator.SetIKPosition(AvatarIKGoal.RightHand, TargetRope.TransformPoint(RightHandTarget));
    //     }
    //     animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandRot);
    //     animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandRot);
    // }
}