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
    public void SetJump()
    {
        AnimationController.SetTrigger("Jump");
    }
    public void SetMantling()
    {
        AnimationController.SetTrigger("Mantling");
    }
}
