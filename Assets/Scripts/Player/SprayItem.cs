using Unity.VisualScripting;
using UnityEngine;

public class SprayItem : MonoBehaviour
{
    [SerializeField] private ParticleSystem Particle;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private Collider AttackRange;
    public float GetDuration()
    {
        if (Particle)
        {
            return Particle.main.duration;
        }
        return 0.0f;
    }
    public void Shoot()
    {
        // AttackRange.enabled = true;
        Particle.Play();
        Audio.Play();
    }
    void OnTriggerEnter(Collider other)
    {

    }
}
