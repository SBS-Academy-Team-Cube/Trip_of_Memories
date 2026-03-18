using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public UnityEvent OnInteractPressed = new UnityEvent(); // 이벤트생성


    public void OnInteract(InputValue Value)
    {
        if(Value.isPressed)
        {
            OnInteractPressed.Invoke();//이벤트 발동
            Debug.Log("Interact Event call");
        }
    }
}
