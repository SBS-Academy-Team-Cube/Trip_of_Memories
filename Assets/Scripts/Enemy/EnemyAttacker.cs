using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField] private string PlayerTag = "Player";
    public bool IsPlayerInAttackRange = false;
    public bool IsAttacking = false;
    public GameObject Player = null;
    public bool CanAttack()
    {
        return IsPlayerInAttackRange && Player != null;
    }
    public void BeginAttack()
    {
        IsAttacking = true;
    }
    public void EndAttack()
    {
        IsAttacking = false;
    }
    private void OnTriggerStay(Collider Other)
    {
        if (IsPlayerInAttackRange)
        {
            return;
        }

        if (Other.CompareTag(PlayerTag))
        {
            IsPlayerInAttackRange = true;
            Player = Other.gameObject;
        }
    }

    private void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag(PlayerTag))
        {
            IsPlayerInAttackRange = false;
            Player = null;
            EndAttack();
        }
    }
    private void Update()
    {
        if (IsAttacking && IsPlayerInAttackRange && Player != null)
        {
            if (Player.TryGetComponent(out Health Hp))
            {
                EndAttack();
                Hp.TakeDamage();
            }
        }
    }
}
