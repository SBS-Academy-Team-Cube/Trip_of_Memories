using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        if(other.gameObject.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage();
        }
        if(other.gameObject.TryGetComponent<PlayerFallRespawner>(out var respawner))
        {
            respawner.Fall();
        }
    }
}
