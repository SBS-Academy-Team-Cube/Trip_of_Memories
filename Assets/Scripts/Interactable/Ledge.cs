using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Ledge : MonoBehaviour
{
    [SerializeField] private SplineContainer Ledges;
    void Awake()
    {
        if (Ledges == null)
        {

        }
        // Ledges.Splines
    }

    public bool TryGetLedgePoint(Vector3 worldPoint, out Vector3 nearestPoint, out float distance)
    {
        nearestPoint = Vector3.zero;
        distance = float.PositiveInfinity;

        if (Ledges == null || Ledges.Splines.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < Ledges.Splines.Count; i++)
        {
            Spline spline = Ledges.Splines[i];

            using var nativeSpline = new NativeSpline(
                spline,
                Ledges.transform.localToWorldMatrix);

            float currentDistance = SplineUtility.GetNearestPoint(
                nativeSpline,
                worldPoint,
                out float3 currentNearestPoint,
                out float currentT,
                resolution: 8,
                iterations: 2);

            if (currentDistance < distance)
            {
                distance = currentDistance;
                nearestPoint = currentNearestPoint;
            }
            // Debug.Log($"{i + 1}th Spline : {currentT}% Point - {currentDistance} (Current Candidate {nearestPoint})");

        }
        return true;
    }
}
