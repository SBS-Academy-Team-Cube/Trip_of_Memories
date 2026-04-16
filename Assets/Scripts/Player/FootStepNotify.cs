using System.Collections.Generic;
using UnityEngine;

public class FootStepNotify : MonoBehaviour
{
    [SerializeField] private List<AudioClip> FootStepClips;
    public void Play()
    {
        if (AudioManager.Instance != null && FootStepClips.Count != 0)
        {
            AudioManager.Instance.PlaySFX(FootStepClips[Random.Range(0, FootStepClips.Count)]);
        }
    }
}
