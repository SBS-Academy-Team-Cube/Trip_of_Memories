using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    [SerializeField] private Animator Animation;

    public void SetTalking()   
    {
        Animation.SetTrigger("Talking");
    }
}
