using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    Animator AnimationController;
    void Awake()
    {
        AnimationController = GetComponent<Animator>();
    }

    public void SetSpeed(float Speed)
    {
        AnimationController.SetFloat("Speed", Speed);
    }
    public void SetJump(bool Jumping)
    {
        AnimationController.SetTrigger("Jump");
    }
}
