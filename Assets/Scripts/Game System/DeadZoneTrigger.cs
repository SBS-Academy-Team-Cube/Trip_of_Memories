using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider Other)
    {
        if (!Other.CompareTag("Player"))
            return;
    
        if(Other.TryGetComponent<Health>(out var Health))
        {
            Health.TakeDamage();
        }
        if(Other.TryGetComponent<PlayerFallRespawner>(out var Respawner))
        {
            Respawner.Fall();
        }
    }
}
