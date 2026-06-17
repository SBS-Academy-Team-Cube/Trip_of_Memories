using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SkyRotation : MonoBehaviour
{
    [SerializeField] private Volume Volume;
    private HDRISky hdriSky;
    public float RotateSpeed = 10.0f;
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
    }

}
