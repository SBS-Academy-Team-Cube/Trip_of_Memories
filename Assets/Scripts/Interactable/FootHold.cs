using UnityEngine;

public class FootHold : MonoBehaviour
{
    [SerializeField] private Mover Effect;
    [SerializeField] private Mover Target;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private Collider TargetItemTrigger;

    void OnEnable()
    {
        if (TargetItemTrigger)
        {
            TargetItemTrigger.enabled = false;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (Effect != null)
        {
            Effect.Move();
        }
        if (Target != null)
        {
            Target.Move();
        }
        if (AudioManager.Instance != null && SFX != null)
        {
            AudioManager.Instance.PlaySFX(SFX, 0.75f);
        }
        if (TargetItemTrigger)
        {
            TargetItemTrigger.enabled = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (Effect != null)
        {
            Effect.Move();
        }
        if (Target != null)
        {
            Target.Move();
        }
        if (TargetItemTrigger)
        {
            TargetItemTrigger.enabled = false;
        }
        if (AudioManager.Instance != null && SFX != null)
        {
            AudioManager.Instance.PlaySFX(SFX, 0.75f);
        }
    }
}
