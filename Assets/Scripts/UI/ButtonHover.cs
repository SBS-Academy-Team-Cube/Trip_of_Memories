using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private RectTransform Rect;
    [SerializeField] private UnderlineEffect Effect;
    float EffectTargetPosY;
    private void Awake()
    {
        EffectTargetPosY = Rect.anchoredPosition.y - Rect.rect.height / 2;
    }
    public void OnPointerEnter(PointerEventData EventData)
    {
        Effect.Play(EffectTargetPosY);
    }
    public void OnPointerExit(PointerEventData EventData)
    {
        Effect.Stop();
    }
}
