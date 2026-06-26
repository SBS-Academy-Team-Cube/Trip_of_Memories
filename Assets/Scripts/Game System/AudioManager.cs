using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioMixerGroup BGMOutputGroup;
    [SerializeField] private AudioMixerGroup SFXOutputGroup;
    [SerializeField] private AudioMixerSnapshot NormalSnapshot;
    [SerializeField] private AudioMixerSnapshot MiniGameSnapshot;
    [SerializeField] private string BGMVolumeParameter = "BGMVolume";
    [SerializeField] private string SFXVolumeParameter = "SFXVolume";
    [SerializeField] private float SnapshotTransitionDuration = 0.25f;

    [Header("BGM")]
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private float BGMFadeOutDuration = 1.0f;
    [SerializeField] private float BGMFadeInDuration = 1.0f;

    [Header("SFX")]
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioClip ButtonClickSFX;

    public event Action OnSFXVolumeBaseChanged;
    private float SFXVolumeMultiplier = 1.0f;
    public float SFX_VOLUME => SFXVolumeMultiplier;
    private float BGMVolumeMultiplier = 1.0f;
    public float BGM_VOLUME => BGMVolumeMultiplier;
    private float CurrentBGMVolumeBase = 1.0f;
    private Coroutine BGMFadeCoroutine;

    protected override void Awake()
    {
        base.Awake();
        ApplyOutputGroups();
        ApplyMixerVolume(BGMVolumeParameter, BGMVolumeMultiplier);
        ApplyMixerVolume(SFXVolumeParameter, SFXVolumeMultiplier);
        UIEventBus.OnAnyButtonClicked += PlayButtonClick;
    }
    private void OnDestroy()
    {
        UIEventBus.OnAnyButtonClicked -= PlayButtonClick;
    }
    public void SetBGMVolume(float Volume)
    {
        BGMVolumeMultiplier = Mathf.Clamp01(Volume);
        if (Mixer != null)
        {
            ApplyMixerVolume(BGMVolumeParameter, BGMVolumeMultiplier);
            return;
        }

        if (BgmSource != null && BGMFadeCoroutine == null)
        {
            BgmSource.volume = CurrentBGMVolumeBase * BGMVolumeMultiplier;
        }
    }
    public void SetSFXVolume(float Volume)
    {
        SFXVolumeMultiplier = Mathf.Clamp01(Volume);
        ApplyMixerVolume(SFXVolumeParameter, SFXVolumeMultiplier);
        OnSFXVolumeBaseChanged?.Invoke();
    }
    public void TransitionToNormalSnapshot()
    {
        TransitionToSnapshot(NormalSnapshot, SnapshotTransitionDuration);
    }
    public void TransitionToMiniGameSnapshot()
    {
        TransitionToSnapshot(MiniGameSnapshot, SnapshotTransitionDuration);
    }
    public void TransitionToNormalSnapshot(float Duration)
    {
        TransitionToSnapshot(NormalSnapshot, Duration);
    }
    public void TransitionToMiniGameSnapshot(float Duration)
    {
        TransitionToSnapshot(MiniGameSnapshot, Duration);
    }
    // ------------------------
    // BGM
    // ------------------------
    public void PlayBGM(AudioClip Clip, float VolumeBase = 1.0f)
    {
        if (Clip == null || BgmSource == null)
        {
            return;
        }
        CurrentBGMVolumeBase = VolumeBase;
        if (BgmSource.clip == Clip)
        {
            BgmSource.loop = true;
            if (!BgmSource.isPlaying)
            {
                BgmSource.Play();
            }
            StartBGMFade(BgmSource.volume, GetBGMSourceVolume(VolumeBase), BGMFadeInDuration, false);
            return;
        }

        StopBGMFade();
        BgmSource.volume = 0.0f;
        BgmSource.clip = Clip;
        BgmSource.loop = true;
        BgmSource.Play();
        StartBGMFade(0.0f, GetBGMSourceVolume(VolumeBase), BGMFadeInDuration, false);
    }
    public float StopBGM(bool bKeepPlayback = false)
    {
        if (BgmSource == null || BgmSource.clip == null)
        {
            return 0.0f;
        }

        StartBGMFade(BgmSource.volume, 0.0f, BGMFadeOutDuration, !bKeepPlayback);
        return BGMFadeOutDuration;
    }

    private void StartBGMFade(float StartVolume, float EndVolume, float Duration, bool bStopAfterFade)
    {
        StopBGMFade();
        BGMFadeCoroutine = StartCoroutine(BGMFade(StartVolume, EndVolume, Duration, bStopAfterFade));
    }

    private void StopBGMFade()
    {
        if (BGMFadeCoroutine != null)
        {
            StopCoroutine(BGMFadeCoroutine);
            BGMFadeCoroutine = null;
        }
    }

    private IEnumerator BGMFade(float StartVolume, float EndVolume, float Duration, bool bStopAfterFade)
    {
        if (Duration <= 0.0f)
        {
            BgmSource.volume = EndVolume;
            if (bStopAfterFade)
            {
                BgmSource.Stop();
            }
            BGMFadeCoroutine = null;
            yield break;
        }

        float Timer = 0.0f;
        while (Timer < Duration)
        {
            Timer += Time.unscaledDeltaTime;
            BgmSource.volume = Mathf.SmoothStep(StartVolume, EndVolume, Timer / Duration);
            yield return null;
        }

        BgmSource.volume = EndVolume;
        if (bStopAfterFade)
        {
            BgmSource.Stop();
        }
        BGMFadeCoroutine = null;
    }
    // ------------------------
    // SFX
    // ------------------------
    public void PlaySFX(AudioClip Clip)
    {
        if (Clip == null || SFXSource == null)
        {
            return;
        }
        SFXSource.volume = Mixer != null ? 1.0f : SFXVolumeMultiplier;
        SFXSource.PlayOneShot(Clip);
    }
    public void PlaySFX(AudioClip Clip, float VolumeBase)
    {
        if (Clip == null || SFXSource == null)
        {
            return;
        }
        SFXSource.volume = Mixer != null ? 1.0f : SFXVolumeMultiplier;
        SFXSource.PlayOneShot(Clip, Mathf.Clamp01(VolumeBase));
    }
    private void PlayButtonClick()
    {
        PlaySFX(ButtonClickSFX);
    }

    private void ApplyOutputGroups()
    {
        if (BgmSource != null && BGMOutputGroup != null)
        {
            BgmSource.outputAudioMixerGroup = BGMOutputGroup;
        }

        if (SFXSource != null && SFXOutputGroup != null)
        {
            SFXSource.outputAudioMixerGroup = SFXOutputGroup;
        }
    }

    private float GetBGMSourceVolume(float VolumeBase)
    {
        float ClampedVolumeBase = Mathf.Clamp01(VolumeBase);
        return Mixer != null ? ClampedVolumeBase : ClampedVolumeBase * BGMVolumeMultiplier;
    }

    private void ApplyMixerVolume(string ParameterName, float LinearVolume)
    {
        if (Mixer == null || string.IsNullOrEmpty(ParameterName))
        {
            return;
        }

        Mixer.SetFloat(ParameterName, LinearToDecibel(LinearVolume));
    }

    private static float LinearToDecibel(float LinearVolume)
    {
        return LinearVolume <= 0.0001f ? -80.0f : Mathf.Log10(LinearVolume) * 20.0f;
    }

    private static void TransitionToSnapshot(AudioMixerSnapshot Snapshot, float Duration)
    {
        if (Snapshot != null)
        {
            Snapshot.TransitionTo(Mathf.Max(0.0f, Duration));
        }
    }
}
