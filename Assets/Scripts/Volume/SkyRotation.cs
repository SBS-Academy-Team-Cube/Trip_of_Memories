using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SkyRotation : MonoBehaviour
{
    [SerializeField] private Volume Volume;
    [SerializeField] private Light directionalLight;
    [SerializeField, Min(0.1f)] private float environmentUpdateInterval = 2.0f;
    [SerializeField, Min(0.1f)] private float environmentUpdateAngle = 5.0f;

    private HDRISky hdriSky;
    private float InitialX;
    private float lastEnvironmentUpdateTime;
    private float lastEnvironmentUpdateRotation;

    public float RotateSpeed = 10.0f;

    private void Awake()
    {
        if (Volume != null && Volume.profile != null && Volume.profile.TryGet(out hdriSky))
        {
            Debug.Log("Find HDRI Sky");
            lastEnvironmentUpdateRotation = hdriSky.rotation.value;
        }

        if (directionalLight != null)
        {
            InitialX = directionalLight.transform.eulerAngles.x;
        }
    }

    private void Update()
    {
        if (hdriSky == null)
        {
            return;
        }
        
        hdriSky.rotation.overrideState = true;

        float newRotation = hdriSky.rotation.value + Time.deltaTime * RotateSpeed;
        if (newRotation >= 360f)
        {
            newRotation -= 360f;
        }

        hdriSky.rotation.value = newRotation;

        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(InitialX, newRotation, 0f);
        }

        if (ShouldUpdateEnvironment(newRotation))
        {
            DynamicGI.UpdateEnvironment();
            lastEnvironmentUpdateTime = Time.time;
            lastEnvironmentUpdateRotation = newRotation;
        }
    }

    private bool ShouldUpdateEnvironment(float currentRotation)
    {
        if (Time.time - lastEnvironmentUpdateTime >= environmentUpdateInterval)
        {
            return true;
        }

        return Mathf.Abs(Mathf.DeltaAngle(lastEnvironmentUpdateRotation, currentRotation)) >= environmentUpdateAngle;
    }
}
