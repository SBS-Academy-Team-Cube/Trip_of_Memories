using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInteraction InteractionComponent;
    void Awake()
    {
        InteractionComponent = GetComponent<PlayerInteraction>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(InputValue Value)
    {
        if(Value.isPressed)
        {
            Debug.Log("Interaction Key Downed");
            
            if(InteractionComponent)
            {
                // 
            }
        }
    }
}
