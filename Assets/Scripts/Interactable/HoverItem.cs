using UnityEngine;

public class HoverItem : MonoBehaviour
{
    [Header("Hover Setting")]
    [SerializeField] private float Amplitude = 0.25f;   // 위아래 이동 범위
    [SerializeField] private float Frequency = 1.5f;    // 진동 속도

    [Header("Rotation Setting")]
    [SerializeField] private float RotateSpeed = 45f;   // 초당 회전 속도 (deg)
    [SerializeField] private Vector3 RotationLocalAxis = Vector3.up;
    private Vector3 StartLocalPos;
    private float TimeOffset;

    private void Awake()
    {
        TimeOffset = Random.Range(0f, 100f);
    }

    private void OnEnable()
    {
        StartLocalPos = transform.localPosition;
    }

    private void Update()
    {
        Hover();
        Rotate();
    }
    private void Hover()
    {
        float OffsetY = Mathf.Sin((Time.time + TimeOffset) * Frequency) * Amplitude;
        transform.localPosition = StartLocalPos + new Vector3(0f, OffsetY, 0f);
    }
    private void Rotate()
    {
        transform.Rotate(RotationLocalAxis * RotateSpeed * Time.deltaTime, Space.Self);
    }
}
