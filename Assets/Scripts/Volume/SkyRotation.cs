using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SkyRotation : MonoBehaviour
{
    [SerializeField] private Volume Volume;
    [SerializeField] private Light directionalLight;
    private HDRISky hdriSky;
    private float InitialX;
    public float RotateSpeed = 10.0f;
    private void Awake()
    {
        if(Volume.profile.TryGet(out hdriSky))
        {
            Debug.Log("Find HDRI Sky");
        }
        Vector3 euler = directionalLight.transform.eulerAngles;
        InitialX = euler.x;
    }
    private void Update()
    {
        if (hdriSky)
        {
            hdriSky.rotation.overrideState = true;

            float newRotation = hdriSky.rotation.value + Time.deltaTime * RotateSpeed;
            if (newRotation >= 360f) newRotation -= 360f;

            hdriSky.rotation.value = newRotation;

            if (directionalLight)
            {
                directionalLight.transform.rotation = Quaternion.Euler(InitialX, newRotation, 0f);
            }
            DynamicGI.UpdateEnvironment();
        }
    }

}
