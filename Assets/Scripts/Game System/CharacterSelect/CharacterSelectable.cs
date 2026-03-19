using System.Collections;
using UnityEngine;

public class CharacterSelectable : MonoBehaviour
{
    public int MyIndex;
    private Animator Animation;
    private Vector3 OriginalPosition;
    private Vector3 TargetPosition;
    private CharacterSelectController Controller;
    private Collider Collider;
    private bool IsSelected = false;
    [SerializeField] private float MoveDistance = 1.0f;
    [SerializeField] private float MoveDuration = 0.8f;

    void Awake()
    {
        OriginalPosition = transform.position;
        TargetPosition = OriginalPosition + transform.forward * MoveDistance;
        if (!TryGetComponent(out Animation))
        {
            Debug.Log($"Can't find Animation in {gameObject.name}");
        }
        Controller = FindFirstObjectByType<CharacterSelectController>();
        if (!TryGetComponent(out Collider))
        {
            Debug.Log($"Can't find Collider in {gameObject.name}");
        }
        if (Controller)
        {
            Controller.OnCharacterSelected += OnSelected;
            Controller.OnSelectionCanceled += OnCanceled;
        }
    }
    private void OnSelected(CharacterSelectable Other)
    {
        if (Other == this)
        {
            StartCoroutine(PlaySelect(TargetPosition));
            IsSelected = true;
        }
        Collider.enabled = false;
    }
    private void OnCanceled()
    {
        if (IsSelected)
        {
            StartCoroutine(PlaySelect(OriginalPosition));
            IsSelected = false;
        }
        Collider.enabled = true;
    }
    public IEnumerator PlaySelect(Vector3 Position)
    {
        Animation.SetFloat("Speed", 10.0f);
        yield return MoveTo(Position);
        Animation.SetFloat("Speed", 0.0f);
    }
    IEnumerator MoveTo(Vector3 target)
    {
        Vector3 Start = transform.position;
        float time = 0f;
        while (time < MoveDuration)
        {
            time += Time.deltaTime;
            float t = time / MoveDuration;

            transform.position = Vector3.Lerp(Start, target, t);
            yield return null;
        }
        transform.position = target;
    }
}
