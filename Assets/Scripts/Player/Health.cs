using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int MaxHP;
    public int MaxHealth => MaxHP;
    public int HP { get; private set; }
    public event Action<int> OnHPChanged;
    public event Action OnDead;
    private void Awake()
    {
        HP = MaxHP;
    }
    public void Init(int Health)
    {
        HP = Mathf.Clamp(Health, 0, MaxHP);
    }
    public void TakeDamage()
    {
        HP--;
        if (HP == 0)
        {
            OnHPChanged?.Invoke(HP);
            OnDead?.Invoke();
        }
        else if (HP > 0)
        {
            OnHPChanged?.Invoke(HP);
        }
    }
    public void Reset()
    {
        HP = MaxHP;
        OnHPChanged?.Invoke(HP);
    }
}
