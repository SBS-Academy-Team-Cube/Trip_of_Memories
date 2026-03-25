using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public enum ETileType { Red, Blue, Green, };
public class RotatingTile : MonoBehaviour
{
    public float RotateDuration = 0.5f;
    public bool CanRotate = true;
    public int RotationIndex;
    public ETileType Type;
    public List<Material> Materials;

    [SerializeField] private GameObject RoadPrefab;
    [SerializeField] private MeshRenderer BaseRenderer;
    [SerializeField] private MeshRenderer RoadRenderer;
    public void Init(int InRotationIndex, ETileType InType)
    {
        RotationIndex = InRotationIndex;
        Type = InType;
        transform.rotation = Quaternion.Euler(0, RotationIndex * 90f, 0);
        BaseRenderer.material = Materials[(int)Type];
        RoadRenderer.material = Materials[(int)Type];

    }
    public void DoRotate(ETileType ButtonType)
    {
        if (!CanRotate)
        {
            return;
        }
        switch (Type)
        {
            case ETileType.Red:
                if (ButtonType == ETileType.Red)
                {
                    RotationIndex = (RotationIndex + 1) % 4;
                }
                break;
            case ETileType.Blue:
                if (ButtonType == ETileType.Blue)
                {
                    RotationIndex = RotationIndex - 1 < 0 ? 3 : (RotationIndex - 1) % 4;
                }
                break;
            case ETileType.Green:
                RotationIndex = ButtonType == ETileType.Red ? (RotationIndex + 1) % 4 : RotationIndex - 1 < 0 ? 3 : (RotationIndex - 1) % 4;
                break;
        }
        StartCoroutine(Rotate(Quaternion.Euler(0, RotationIndex * 90f, 0)));
    }
    private IEnumerator Rotate(Quaternion TargetRotation)
    {
        CanRotate = false;
        Quaternion StartRotation = transform.rotation;
        float Elapsed = 0f;

        while (Elapsed <= RotateDuration)
        {
            Elapsed += Time.deltaTime;
            float t = Elapsed / RotateDuration;
            transform.rotation = Quaternion.Slerp(StartRotation, TargetRotation, t);
            yield return null;
        }
        transform.rotation = TargetRotation;
        CanRotate = true;
    }
}

