using UnityEngine;
using UnityEngine.UI;

public class UIVolumePanel : MonoBehaviour
{
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void OnEnable()
    {
        if (AudioManager.Instance && BGMSlider && SFXSlider)
        {
            BGMSlider.value = AudioManager.Instance.BGM_VOLUME;
            SFXSlider.value = AudioManager.Instance.SFX_VOLUME;

            BGMSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
            SFXSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        }
    }
    private void OnDisable()
    {
        if (AudioManager.Instance && BGMSlider && SFXSlider)
        {
            BGMSlider.onValueChanged.RemoveAllListeners();
            SFXSlider.onValueChanged.RemoveAllListeners();
        }
    }
}
