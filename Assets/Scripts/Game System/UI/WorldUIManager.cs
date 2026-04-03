using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class WorldUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text UIText;
    [SerializeField] Transform CameraTransform;
    void Awake()
    {
        FindFirstObjectByType<PlayerInteraction>()?.
    }

    void Start()
    {

    }
    public void SetText(string Text)
    {
        UIText.text = Text;
    }

    private void LateUpdate()
    {
        if(transform == null)
        {
            Debug.Log("No Transform");
        }
        transform.forward = CameraTransform.forward;
    }
}
