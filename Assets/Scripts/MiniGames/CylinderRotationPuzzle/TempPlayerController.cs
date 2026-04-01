using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayerController : MonoBehaviour
{
    private TempPlayerMove Movement;

    private void Awake()
    {
        Movement = GetComponent<TempPlayerMove>();
    }
    public void OnMove(InputValue Value)
    {
        if (Movement != null)
        {
            Movement.TryMove(Value);
        }
    }

}
