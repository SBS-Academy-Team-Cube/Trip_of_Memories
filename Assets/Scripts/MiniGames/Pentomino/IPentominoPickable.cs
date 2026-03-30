using UnityEngine;

public interface IPentominoPickable
{
    void PickUp();
    void Place(Vector3 pos);
    bool IsPicked { get;}
    Transform Transform { get;}
    Bounds GetBounds();
}
