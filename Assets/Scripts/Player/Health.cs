using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int MaxHP;
    public int HP;
    public event Action<int> OnHPChanged;
    public event Action OnDead;

    private void Awake()
    {
        HP = MaxHP;
    }
    
    public void TakeDamage()
    {
        Debug.Log($"{name} : be Hit!!");
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
        OnHPChanged?.Invoke(HP);
    }
}
