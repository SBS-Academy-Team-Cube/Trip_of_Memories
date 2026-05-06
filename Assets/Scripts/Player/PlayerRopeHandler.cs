using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRopeHandler : MonoBehaviour
{
    [SerializeField] private PlayerState State;
    [SerializeField] private PlayerAnimation Animation;
    public UnityEvent<bool> OnEnterHanging;
    public float Speed;
    public void GrapRope(GameObject Target)
    {
        float clampedY;
        if (Target.TryGetComponent(out Renderer renderer))
        {
            Bounds bounds = renderer.bounds;
            clampedY = Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y);
        }
        else
        {
            return;
        }
        OnEnterHanging?.Invoke(true);
        Animation.SetHangOnRope(true);
        if (State)
        {
            State.SetAction(PlayerState.EAction.Hanging);
        }
        if (TryGetComponent(out CharacterController cc))
        {
            cc.enabled = false;
            transform.position = new Vector3(Target.transform.position.x, clampedY, Target.transform.position.z);
            cc.enabled = true;
        }
    }
    public void ReleaseRope()
    {
        if (State)
        {
            State.SetAction(PlayerState.EAction.None);
        }
        OnEnterHanging?.Invoke(false);
        Animation.SetHangOnRope(false);
    }
}