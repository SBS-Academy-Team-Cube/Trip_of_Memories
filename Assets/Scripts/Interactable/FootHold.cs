using UnityEngine;
using System.Collections.Generic;

public class FootHold : MonoBehaviour
{
    [SerializeField] private Mover Effect;
    [SerializeField] private Mover Target;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private Collider TargetItemTrigger;
    private readonly Dictionary<Transform, int> PresserColliderCounts = new();

    void OnEnable()
    {
        PresserColliderCounts.Clear();

        if (TargetItemTrigger)
        {
            TargetItemTrigger.enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Transform PresserRoot = GetPresserRoot(other);
        Debug.Log($"{other.name} Entered");
        int PreviousRootCount = PresserColliderCounts.Count;

        if (PresserColliderCounts.TryGetValue(PresserRoot, out int ColliderCount))
        {
            PresserColliderCounts[PresserRoot] = ColliderCount + 1;
        }
        else
        {
            PresserColliderCounts.Add(PresserRoot, 1);
        }

        if (PreviousRootCount == 0 && PresserColliderCounts.Count == 1)
        {
            SetPressed(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Transform PresserRoot = GetPresserRoot(other);
        Debug.Log($"{PresserRoot.name} Exit");

        if (!PresserColliderCounts.TryGetValue(PresserRoot, out int ColliderCount))
        {

            return;
        }

        ColliderCount--;

        if (ColliderCount > 0)
        {
            PresserColliderCounts[PresserRoot] = ColliderCount;
            return;
        }

        PresserColliderCounts.Remove(PresserRoot);
        if (PresserColliderCounts.Count == 0)
        {

            SetPressed(false);
        }
    }

    private static Transform GetPresserRoot(Collider Other)
    {
        return Other.attachedRigidbody ? Other.attachedRigidbody.transform.root : Other.transform.root;
    }

    private void SetPressed(bool Pressed)
    {
        if (Effect != null)
        {
            Effect.Move();
        }
        if (Target != null)
        {
            Target.Move();
        }
        if (TargetItemTrigger)
        {
            TargetItemTrigger.enabled = Pressed;
        }
        if (AudioManager.Instance != null && SFX != null)
        {
            AudioManager.Instance.PlaySFX(SFX, 0.75f);
        }
    }
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (Effect != null)
    //     {
    //         Effect.Move();
    //     }
    //     if (Target != null)
    //     {
    //         Target.Move();
    //     }
    //     if (AudioManager.Instance != null && SFX != null)
    //     {
    //         AudioManager.Instance.PlaySFX(SFX, 0.75f);
    //     }
    //     if (TargetItemTrigger)
    //     {
    //         TargetItemTrigger.enabled = true;
    //     }
    // }
    // private void OnCollisionExit(Collision other)
    // {
    //     if (Effect != null)
    //     {
    //         Effect.Move();
    //     }
    //     if (Target != null)
    //     {
    //         Target.Move();
    //     }
    //     if (TargetItemTrigger)
    //     {
    //         TargetItemTrigger.enabled = false;
    //     }
    //     if (AudioManager.Instance != null && SFX != null)
    //     {
    //         AudioManager.Instance.PlaySFX(SFX, 0.75f);
    //     }
    // }
}
