using System;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator AnimationController;
    private int hangingHash;
    // void Awake()
    // {
    //     hangingHash = Animator.StringToHash("Base Layer.RopeMove");
    // }

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
    public void SetIsHanging(bool IsHanging)
    {
        AnimationController.SetTrigger(IsHanging ? "StartHanging" : "EndHanging");
    }

    public void PlayRopeAnimation(bool bReverse)
    {
        AnimatorStateInfo info = AnimationController.GetCurrentAnimatorStateInfo(0);
        Debug.Log(info);
        // if (info.fullPathHash != hangingHash)
        // {
        //     return;
        // }
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
