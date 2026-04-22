using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class temp : MonoBehaviour
{
    [SerializeField] private Button button;

    private RectTransform rectTrans;

    private Vector2 targetPos1 = new Vector2(-900f, 0f);
    private Vector2 targetPos2 = new Vector2(900f, 0f);
    private bool isGoRight = true;
    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
        if (rectTrans == null)
            Debug.Log("rrr");
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Move);
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(Move);
    }
    private void Move()
    {
        if(isGoRight)// 오른쪽으로 가야함
        {
            rectTrans.DOAnchorPos(targetPos2,1.5f).SetEase(Ease.InBounce);
            isGoRight = false;
        }
        else
        {
            rectTrans.DOAnchorPos(targetPos1, 1.5f).SetEase(Ease.InBounce);
            isGoRight = true;
        }
    }
}
