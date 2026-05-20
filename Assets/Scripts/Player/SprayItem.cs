using System.Collections;
using UnityEngine;

public class SprayItem : MonoBehaviour
{
    [SerializeField] private ParticleSystem Particle;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private Collider AttackRange;
    private bool CanAttack = false;
    public float GetDuration()
    {
        if (Particle)
        {
            return Particle.main.duration;
        }
        return 0.0f;
    }
    public void Shoot(Quaternion Direction)
    {
        transform.rotation = Direction;

        AttackRange.enabled = true;
        CanAttack = true;
        StartCoroutine(DisableTrigger());
        Particle.Play();
        Audio.Play();
    }
    void OnTriggerStay(Collider Other)
    {
        if (!CanAttack)
        {
            return;
        }
        if (Other.CompareTag("Enemy"))
        {
            Health EnemyHP = Other.GetComponentInParent<Health>();
            if (EnemyHP != null)
            {
                EnemyHP.TakeDamage();
                CanAttack = false;
            }
        }
    }
    private IEnumerator DisableTrigger()
    {
        yield return new WaitForSeconds(GetDuration());
        AttackRange.enabled = false;
        CanAttack = false;
    }
}
