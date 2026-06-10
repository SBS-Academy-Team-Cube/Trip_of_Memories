using UnityEngine;
using UnityEngine.Events;

public class PlayerRopeHandler : MonoBehaviour
{
    [SerializeField] private PlayerState State;
    [SerializeField] private PlayerAnimation Animation;
    public UnityEvent<bool> OnEnterHanging;
    public float Speed;
    public bool TryGrapRope(GameObject Target)
    {
        if (!Target.TryGetComponent(out Renderer renderer))
        {
            return false;
        }
        Bounds bounds = renderer.bounds;
        float clampedY = Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y);

        OnEnterHanging?.Invoke(true);
        Animation.SetHangOnRope(true);
        if (State != null)
        {
            State.SetInterAction(PlayerState.EInterAction.Hanging);
        }
        if (TryGetComponent(out CharacterController cc))
        {
            cc.enabled = false;
            transform.position = new Vector3(Target.transform.position.x, clampedY, Target.transform.position.z);
            cc.enabled = true;
        }
        return true;
    }
    public void ReleaseRope()
    {
        if (State)
        {
            State.SetInterAction(PlayerState.EInterAction.None);
        }
        OnEnterHanging?.Invoke(false);
        Animation.SetHangOnRope(false);
    }
}