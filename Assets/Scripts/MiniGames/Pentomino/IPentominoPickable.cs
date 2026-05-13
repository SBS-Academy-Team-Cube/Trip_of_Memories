using UnityEngine;

public interface IPentominoPickable
{
    void PickUp(float gridSize);
    void Place(Vector3 pos);
    bool IsPicked { get;}
    Transform Transform { get;}
}
