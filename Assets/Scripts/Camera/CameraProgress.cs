using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System;

public class CameraProgress : MonoBehaviour
{
    public Transform player;

    public SplineContainer spline;
    public CinemachineSplineDolly DollyCamera;
    private float CurrentT;

    [SerializeField]
    private float CameraLerpSpeed = 5f;

    void LateUpdate()
    {
        float3 PlayerPosition = player.position;
        
        SplineUtility.GetNearestPoint(
            spline.Spline,
            PlayerPosition,
            out float3 nearest,
            out float t
        );
        CurrentT = Mathf.Lerp(CurrentT, t, Time.deltaTime * CameraLerpSpeed);
        DollyCamera.CameraPosition = CurrentT;
    }
}