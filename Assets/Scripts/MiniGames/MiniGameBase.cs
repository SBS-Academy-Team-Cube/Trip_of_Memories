using System;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    [SerializeField] protected string MiniGameID;
    [SerializeField] protected int MemoryRecoveryAmount;
    public abstract event Action OnPlay;
    public abstract event Action OnClear;
    public abstract event Action OnFail;

    public abstract void Play();
    public abstract void Clear();
    public abstract void Fail();
    public abstract bool HasCleared();
}