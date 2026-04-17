using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRopeHandler : MonoBehaviour
{
    public UnityEvent<bool> OnEnterHanging;
    [SerializeField] private CharacterController Controller;
    Vector2 Move;
    public float Speed;
    public void GrapRope(GameObject TargetRope)
    {
        Debug.Log($"Rope Position {TargetRope.transform.position}\nCharacter Position {transform.position}");
        transform.position = TargetRope.transform.position;
        Debug.Log($"Rope Position {TargetRope.transform.position}\nCharacter Position {transform.position}");
        OnEnterHanging?.Invoke(true);
    }
    public void TryMove(Vector2 Value)
    {
        Move = Value;
    }
    private void Update()
    {
        if (Controller)
        {
            if (Move.y != 0)
            {
                Controller.Move(Speed * Time.deltaTime * Vector3.ClampMagnitude(Vector3.up * Move.y, 1f));
            }
            else
            {
                Controller.Move(Vector3.zero);
            }
        }
    }
}