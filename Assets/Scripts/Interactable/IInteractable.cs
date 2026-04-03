using UnityEngine;

public interface IInteractable
{
    string GetInteractionPrompt();
    bool Interact(GameObject Interactor);
    Transform GetTransform(); 
}
