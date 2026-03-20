using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System;
using Unity.VisualScripting;

public class CameraProgress : MonoBehaviour
{
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private SplineContainer spline;
    [SerializeField] private CinemachineSplineDolly DollyCamera;
    [SerializeField] private CinemachineCamera CineMachine;
    [SerializeField] private PlayerSpawner Spawner;
    [SerializeField] private float CurrentT;

    [SerializeField]
    private float CameraLerpSpeed = 5f;

    private void OnEnable()
    {
        Spawner.OnPlayerSpawned += OnPlayerSpawned;
    }
    private void OnPlayerSpawned(GameObject Player)
    {
        if(Player.TryGetComponent(out PlayerMovement Movement))
        {
            PlayerTransform = Movement.transform;
            CineMachine.Target.TrackingTarget = PlayerTransform;
        }

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