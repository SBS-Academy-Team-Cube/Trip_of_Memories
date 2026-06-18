using UnityEngine;
using UnityEngine.Events;
public class PlayerDragHandler : MonoBehaviour
{
    public UnityEvent<bool> OnKeyDragging;
    private enum EDraggingState { Released, Pushing, Pulling };
    [SerializeField] private PlayerState State;
    [SerializeField] private PlayerAnimation Animation;
    private GameObject DraggingChild = null;
    public bool TryGrap(GameObject Child, DraggingGrap TargetGrap)
    {
        TargetGrap.GetStandPosition(out Vector3 TargetPosition, out Quaternion TargetRotation);

        if (TryGetComponent(out CharacterController controller))
        {
            controller.enabled = false;
            transform.SetPositionAndRotation(TargetPosition, TargetRotation);
            controller.enabled = true;
        }

        if (State)
        {
            State.SetInterAction(PlayerState.EInterAction.KeyDragging);
        }
        Child.transform.SetParent(transform, true);
        DraggingChild = Child;


        TargetGrap.GetIK(out Transform Right, out Transform Left);
        Animation.SetHandIKTargets(Left, Right);
        Animation.SetHandIKWeight(1.0f, 1.0f);

        Animation.SetDragging((int)EDraggingState.Pushing, 0.0f);
        OnKeyDragging?.Invoke(true);

        return true;
    }
    public void TryRelease()
    {
        if (DraggingChild == null)
        {
            return;
        }
        Animation.ClearHandIK();
        Animation.SetDragging((int)EDraggingState.Released);
        DraggingChild.transform.SetParent(null, true);
        if (State)
        {
            State.SetInterAction(PlayerState.EInterAction.None);
        }
        OnKeyDragging?.Invoke(false);
    }
}
