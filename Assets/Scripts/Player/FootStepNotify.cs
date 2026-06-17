using System.Collections.Generic;
using UnityEngine;

public class FootStepNotify : MonoBehaviour
{
    [SerializeField] private RandomSoundQueue Queue;
    public void PlayFootStep()
    {
        if (AudioManager.Instance != null && Queue != null && !Queue.Empty())
        {
            AudioManager.Instance.PlaySFX(Queue.GetSound());
        }
    }
}
