using UnityEngine;

public class OnStateUpdateSetSpeed : StateMachineBehaviour
{
    public string Speed = "Speed";
    public float moveSpeed = 0f;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(Speed, moveSpeed);
    }
}
