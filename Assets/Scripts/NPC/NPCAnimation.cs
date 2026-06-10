using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    [SerializeField] private Animator Animation;

    public void SetTalking()
    {
        if (!Animation.GetCurrentAnimatorStateInfo(0).IsName("Talking"))
        {
            Animation.SetTrigger("Talking");
        }
    }
}
