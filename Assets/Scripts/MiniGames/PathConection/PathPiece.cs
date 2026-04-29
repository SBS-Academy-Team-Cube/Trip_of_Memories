using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public enum PathColor
{
    Green,
    Blue,
    Red
}
public class PathPiece : MonoBehaviour
{
    [SerializeField] private static float rotateDuration = 1.5f;

    private RectTransform trans;

    public static float RotateDuration => rotateDuration;


    private void Start()
    {
        trans = GetComponent<RectTransform>();
        DOTween.Init();
    }

    public void PieceRoll()
    {
        trans.DOLocalRotate(new Vector3(0f, 0f, -90f), rotateDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic);
    }
}
