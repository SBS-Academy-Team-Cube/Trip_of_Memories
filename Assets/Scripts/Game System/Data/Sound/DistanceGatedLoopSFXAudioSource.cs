using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DistanceGatedLoopSFXAudioSource : MonoBehaviour
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
    [SerializeField] private bool PlayOnEnable = true;
    [SerializeField] private bool UseAudioSourceMaxDistance = true;
    [SerializeField] private float AudibleDistance = 25f;
    [SerializeField] private bool UseDistanceAttenuation = true;
    [SerializeField] private bool UseAudioSourceMinDistance = true;
    [SerializeField] private float FullVolumeDistance = 1f;
    [SerializeField] private AnimationCurve DistanceVolumeCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);
    [SerializeField] private float CheckInterval = 0.5f;
    [SerializeField] private OutOfRangeBehaviour OutOfRangeMode = OutOfRangeBehaviour.Pause;
    [SerializeField] private bool ResumeLoopWhenInRange = true;

    private AudioManager SubscribedAudioManager;
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
        PrepareLoopSource();
        FindListenerIfNeeded();
        SubscribeSFXVolumeChanged();
        RefreshAudibility();
    }

    private void OnEnable()
    {
        CheckTimer = 0f;
        PrepareLoopSource();
        FindListenerIfNeeded();
        SubscribeSFXVolumeChanged();

        if (PlayOnEnable && Source != null && Source.clip != null && !Source.isPlaying)
        {
            Source.Play();
        }

        RefreshAudibility();
    }

    private void OnDisable()
    {
        UnsubscribeSFXVolumeChanged();
    }

    private void OnDestroy()
    {
        UnsubscribeSFXVolumeChanged();
    }

    private void Update()
    {
        SubscribeSFXVolumeChanged();

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
            RestoreInRangeState(Distance);
        }
        else
        {
            ApplyOutOfRangeState();
        }
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

    private void PrepareLoopSource()
    {
        if (Source == null)
        {
            return;
        }

        Source.loop = true;
    }

    private void ApplyOutOfRangeState()
    {
        switch (OutOfRangeMode)
        {
            case OutOfRangeBehaviour.Mute:
                Source.volume = 0f;
                return;
            case OutOfRangeBehaviour.Pause:
                ApplyCurrentVolume();
                if (Source.isPlaying)
                {
                    Source.Pause();
                    WasStoppedByGate = true;
                }
                return;
            case OutOfRangeBehaviour.Stop:
                ApplyCurrentVolume();
                if (Source.isPlaying)
                {
                    Source.Stop();
                    WasStoppedByGate = true;
                }
                return;
        }
    }

    private void RestoreInRangeState(float Distance = 0f)
    {
        if (Source == null)
        {
            return;
        }

        ApplyCurrentVolume(Distance);

        if (!WasStoppedByGate || !ResumeLoopWhenInRange || Source.clip == null)
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

    private void ApplyCurrentVolume(float Distance = 0f)
    {
        float SFXVolumeMultiplier = AudioManager.Instance ? AudioManager.Instance.SFX_VOLUME : 1f;
        Source.volume = Mathf.Clamp01(OriginalVolume * GetDistanceVolumeMultiplier(Distance) * SFXVolumeMultiplier);
    }

    private float GetAudibleDistance()
    {
        if (UseAudioSourceMaxDistance && Source != null)
        {
            return Source.maxDistance;
        }

        return AudibleDistance;
    }

    private float GetFullVolumeDistance()
    {
        if (UseAudioSourceMinDistance && Source != null)
        {
            return Source.minDistance;
        }

        return FullVolumeDistance;
    }

    private float GetDistanceVolumeMultiplier(float Distance)
    {
        if (!UseDistanceAttenuation)
        {
            return 1f;
        }

        float MaxDistance = Mathf.Max(0f, GetAudibleDistance());
        float MinDistance = Mathf.Clamp(GetFullVolumeDistance(), 0f, MaxDistance);

        if (Distance <= MinDistance)
        {
            return 1f;
        }

        if (MaxDistance <= MinDistance)
        {
            return 0f;
        }

        float NormalizedDistance = Mathf.Clamp01((Distance - MinDistance) / (MaxDistance - MinDistance));
        if (DistanceVolumeCurve == null)
        {
            return 1f - NormalizedDistance;
        }

        return Mathf.Clamp01(DistanceVolumeCurve.Evaluate(NormalizedDistance));
    }

    private void HandleSFXVolumeBaseChanged()
    {
        RefreshAudibility();
    }

    private void SubscribeSFXVolumeChanged()
    {
        if (SubscribedAudioManager != null || AudioManager.Instance == null)
        {
            return;
        }

        SubscribedAudioManager = AudioManager.Instance;
        SubscribedAudioManager.OnSFXVolumeBaseChanged += HandleSFXVolumeBaseChanged;
    }

    private void UnsubscribeSFXVolumeChanged()
    {
        if (SubscribedAudioManager == null)
        {
            return;
        }

        SubscribedAudioManager.OnSFXVolumeBaseChanged -= HandleSFXVolumeBaseChanged;
        SubscribedAudioManager = null;
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
