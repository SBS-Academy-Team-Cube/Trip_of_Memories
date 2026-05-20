using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DistanceGatedAudioSource : MonoBehaviour, IAudible3D
{
    private enum OutOfRangeBehaviour
    {
        Mute,
        Pause,
        Stop
    }

    [SerializeField] private AudioSource Source;
    [SerializeField] private Transform Listener;
    [SerializeField] private bool AutoFindListener = true;
    [SerializeField] private bool UseAudioSourceMaxDistance = true;
    [SerializeField] private float AudibleDistance = 25f;
    [SerializeField] private float CheckInterval = 0.2f;
    [SerializeField] private OutOfRangeBehaviour OutOfRangeMode = OutOfRangeBehaviour.Pause;
    [SerializeField] private bool ResumeLoopWhenInRange = true;

    private float OriginalVolume;
    private float CheckTimer;
    private bool WasStoppedByGate;

    public bool IsAudible => Source != null && Source.enabled && Source.gameObject.activeInHierarchy && IsWithinAudibleRange;
    public bool IsWithinAudibleRange { get; private set; } = true;

    private void Awake()
    {
        if (Source == null)
        {
            Source = GetComponent<AudioSource>();
        }

        OriginalVolume = Source != null ? Source.volume : 1f;
        FindListenerIfNeeded();
        RefreshAudibility();
    }

    private void OnEnable()
    {
        CheckTimer = 0f;
        FindListenerIfNeeded();
        RefreshAudibility();
    }

    private void Update()
    {
        CheckTimer -= Time.deltaTime;
        if (CheckTimer > 0f)
        {
            return;
        }

        CheckTimer = CheckInterval;
        FindListenerIfNeeded();
        RefreshAudibility();
    }

    public void RefreshAudibility()
    {
        if (Source == null || Listener == null)
        {
            IsWithinAudibleRange = true;
            RestoreInRangeState();
            return;
        }

        float Distance = Vector3.Distance(transform.position, Listener.position);
        IsWithinAudibleRange = Distance <= GetAudibleDistance();

        if (IsWithinAudibleRange)
        {
            RestoreInRangeState();
        }
        else
        {
            ApplyOutOfRangeState();
        }
    }

    public void PlayOneShot(AudioClip Clip)
    {
        if (Clip == null || Source == null)
        {
            return;
        }

        RefreshAudibility();
        if (!IsAudible)
        {
            return;
        }

        Source.PlayOneShot(Clip);
    }

    public void Stop()
    {
        if (Source == null)
        {
            return;
        }

        Source.Stop();
        WasStoppedByGate = false;
    }

    private void ApplyOutOfRangeState()
    {
        switch (OutOfRangeMode)
        {
            case OutOfRangeBehaviour.Mute:
                Source.volume = 0f;
                return;
            case OutOfRangeBehaviour.Pause:
                Source.volume = OriginalVolume;
                if (Source.isPlaying)
                {
                    Source.Pause();
                    WasStoppedByGate = true;
                }
                return;
            case OutOfRangeBehaviour.Stop:
                Source.volume = OriginalVolume;
                if (Source.isPlaying)
                {
                    Source.Stop();
                    WasStoppedByGate = true;
                }
                return;
        }
    }

    private void RestoreInRangeState()
    {
        if (Source == null)
        {
            return;
        }

        Source.volume = OriginalVolume;

        if (!WasStoppedByGate || !ResumeLoopWhenInRange || !Source.loop || Source.clip == null)
        {
            return;
        }

        if (OutOfRangeMode == OutOfRangeBehaviour.Pause)
        {
            Source.UnPause();
        }
        else if (OutOfRangeMode == OutOfRangeBehaviour.Stop)
        {
            Source.Play();
        }

        WasStoppedByGate = false;
    }

    private float GetAudibleDistance()
    {
        if (UseAudioSourceMaxDistance && Source != null)
        {
            return Source.maxDistance;
        }

        return AudibleDistance;
    }

    private void FindListenerIfNeeded()
    {
        if (!AutoFindListener || Listener != null)
        {
            return;
        }

        AudioListener FoundListener = FindFirstObjectByType<AudioListener>();
        if (FoundListener != null)
        {
            Listener = FoundListener.transform;
        }
    }
}
