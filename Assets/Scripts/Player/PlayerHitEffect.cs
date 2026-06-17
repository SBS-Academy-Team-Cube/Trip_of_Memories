using System.Collections.Generic;
using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    [SerializeField] private Health HP;
    [SerializeField] private List<ParticleSystem> Effects;
    private void Awake()
    {
        if (HP == null)
        {
            TryGetComponent(out HP);
        }
        if (Effects.Count <= 0)
        {
            GetComponentsInChildren(Effects);
        }
    }
    private void OnEnable()
    {
        HP.OnHPChanged += HitEffect;
    }
    private void OnDisable()
    {
        HP.OnHPChanged -= HitEffect;
    }
    private void HitEffect(int _)
    {
        foreach (var effect in Effects)
        {
            effect.Play();
        }
    }
}
