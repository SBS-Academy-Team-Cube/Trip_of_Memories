using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System;

public class CameraProgress : MonoBehaviour
{
    private Transform PlayerTransform;
    [SerializeField] private SplineContainer spline;
    [SerializeField] private CinemachineSplineDolly DollyCamera;
    [SerializeField] private PlayerSpawner Spawner;
    private float CurrentT;

    [SerializeField]
    private float CameraLerpSpeed = 5f;

    private void OnEnable()
    {
        Spawner.OnPlayerSpawned += OnPlayerSpawned;
    }
    private void OnPlayerSpawned(GameObject Player)
    {
        // PlayerTransform = Player.CameraPivot;

    }
    void LateUpdate()
    {
        float3 PlayerPosition = PlayerTransform.position;

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