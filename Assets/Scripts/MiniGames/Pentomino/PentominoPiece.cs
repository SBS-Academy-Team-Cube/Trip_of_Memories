using UnityEngine;

[RequireComponent(typeof(PieceHighlighter))]
public class PentominoPiece : MonoBehaviour, IPentominoPickable, IHighlightable
{
    private bool _isPicked = false;
    private PieceHighlighter _highlighter;

    public bool IsPicked => _isPicked;
    public Transform Transform => transform;

    // ★★ 실제 전체 크기 계산 (자식 Cube들의 MeshRenderer를 모두 합침)
    public Bounds GetBounds()
    {
        Bounds combinedBounds = new Bounds(transform.position, Vector3.zero);

        // 자식 오브젝트들의 Collider를 모두 찾아서 Bounds 합치기
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            combinedBounds.Encapsulate(collider.bounds);
        }

        return combinedBounds;
    }

    private void Awake()
    {
        _highlighter = GetComponent<PieceHighlighter>();
    }

    public void PickUp()
    {
        _isPicked = true;
        transform.position += Vector3.up * 0.5f;
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
