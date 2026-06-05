using UnityEngine;

public interface IPentominoPickable
{
    void PickUp(float gridSize);
    void SetPreviewPosition(Vector3 Target);
    void Place(Vector3 pos);
    bool IsPicked { get;}
    Transform Transform { get;}
}
