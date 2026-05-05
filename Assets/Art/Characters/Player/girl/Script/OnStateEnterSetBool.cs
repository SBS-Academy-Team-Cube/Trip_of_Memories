using UnityEngine;

public class OnStateEnterSetBool : StateMachineBehaviour
{
    public string boolName = "MoveLock";
    public bool value = true;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, value);
    }
}
