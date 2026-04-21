using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool bClockWise = true;
    public float RotateSpeed = 30f;

    void Update()
    {
        float direction = bClockWise ? 1f : -1f;
        transform.Rotate(0f, direction * RotateSpeed * Time.deltaTime, 0f);
    }
}
