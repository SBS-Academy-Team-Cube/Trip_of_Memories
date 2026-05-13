using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int MaxHP;
    private int HP;
    public event Action<int> OnHPChanged;
    public event Action OnDead;
    public void TakeDamage()
    {
        HP--;
        if (HP <= 0)
        {
            OnDead?.Invoke();
        }
        else
        {
            OnHPChanged?.Invoke(HP);
        }
    }
    public void Reset()
    {
        HP = MaxHP;
    }
}
