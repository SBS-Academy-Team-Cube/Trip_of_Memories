using UnityEngine;

[RequireComponent(typeof(PieceHighlighter))]
public class PentominoPiece : MonoBehaviour, IPentominoPickable, IHighlightable
{
    private bool _isPicked = false;
    private PieceHighlighter _highlighter;

    public bool IsPicked => _isPicked;
    public Transform Transform => transform;

    private void Awake()
    {
        _highlighter = GetComponent<PieceHighlighter>();
    }

    public void PickUp()
    {
        _isPicked = true;
        transform.position += Vector3.up * 1.5f;
        _highlighter.HighlightOff();
    }

    public void Place(Vector3 position)
    {
        _isPicked = false;
        transform.position = new Vector3(position.x, 0.01f, position.z);
    }

    public void HighlightOn() => _highlighter.HighlightOn();
    public void HighlightOff() => _highlighter.HighlightOff();
}
