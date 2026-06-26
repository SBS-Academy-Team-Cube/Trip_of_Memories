using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [SerializeField] private RectTransform Rect;
    [SerializeField] private UnderlineEffect Effect;
    private void Awake()
    {
        if (Rect == null)
        {
            Rect = transform as RectTransform;
        }
    }
    public void OnPointerEnter(PointerEventData EventData)
    {
        Effect.Play(Rect);
    }
    public void OnPointerExit(PointerEventData EventData)
    {
        Effect.Stop();
    }
    public void OnPointerClick(PointerEventData EventData)
    {
        Effect.Stop();
    }
    private void OnDisable()
    {
        if (Effect != null)
        {
            Effect.Stop();
        }
    }
}
