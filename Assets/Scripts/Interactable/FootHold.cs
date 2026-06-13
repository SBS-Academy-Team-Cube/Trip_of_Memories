using UnityEngine;

public class FootHold : MonoBehaviour
{
    [SerializeField] private Mover Effect;
    [SerializeField] private Mover Target;
    [SerializeField] private AudioClip SFX;
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
        if (AudioManager.Instance != null && SFX != null)
        {
            AudioManager.Instance.PlaySFX(SFX, 0.75f);
        }
    }
}
