using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    public bool IsPlayerInAttackRange = false;
    public bool IsAttacking = false;
    
    public GameObject Player = null;
    public bool CanAttack()
    {
        return IsPlayerInAttackRange;
    }

    private void OnTriggerStay(Collider Other) 
    {   
        if(IsPlayerInAttackRange)
        {
            return;
        }
        if(Other.CompareTag("Player"))
        {
            IsPlayerInAttackRange = true;
            Player = Other.gameObject;
        }
    }
    private void OnTriggerExit(Collider Other)
    {
        if(Other.CompareTag("Player"))
        {
            IsPlayerInAttackRange = false;
            Player = null;
        }
    }

    private void Update() 
    {
        if(IsAttacking && IsPlayerInAttackRange && Player != null)
        {
            // if(Player.TryGetComponent(out Health Hp))
            // {
            //     Hp.TakeDamage();
            // }
            Debug.Log("Attack Player!!");
        }
    }
}
