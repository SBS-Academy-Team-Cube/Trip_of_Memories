using UnityEngine;

public class RendererHider : MonoBehaviour
{
    [SerializeField] private Renderer Renderer;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HiddenBox"))
        {
            Renderer.enabled = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HiddenBox"))
        {
            Renderer.enabled = true;
        }
    }
}
