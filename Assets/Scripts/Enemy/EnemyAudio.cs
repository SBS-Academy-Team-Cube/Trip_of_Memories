using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource Source;
    [SerializeField] private RandomSoundQueue SoundQueueData;
    private RandomSoundQueue RuntimeSoundQueue;
    private void Awake()
    {
        if (Source == null)
        {
            TryGetComponent(out Source);
        }
        if (SoundQueueData != null)
        {
            RuntimeSoundQueue = Instantiate(SoundQueueData);
        }
    }
    public void Play()
    {
        if (Source.isPlaying || Source == null || RuntimeSoundQueue == null)
        {
            return;
        }
        Source.PlayOneShot(RuntimeSoundQueue.GetSound());
    }
}
