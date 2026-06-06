using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private List<AudioClip> BgmList;
    [SerializeField] private float BGMFadeOutDuration = 1.0f;
    [SerializeField] private float BGMFadeInDuration = 1.0f;

    [Header("SFX")]
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioClip ButtonClickSFX;

    private float SFXVolumeMultiplier = 1.0f;
    public float SFX_VOLUME => SFXVolumeMultiplier;
    private float BGMVolumeMultiplier = 1.0f;
    public float BGM_VOLUME => BGMVolumeMultiplier;
    private Coroutine BGMFadeCoroutine;

    protected override void Awake()
    {
        base.Awake();
        UIEventBus.OnAnyButtonClicked += PlayButtonClick;
    }
    private void OnDestroy()
    {
        UIEventBus.OnAnyButtonClicked -= PlayButtonClick;
    }
    public void SetBGMVolume(float Volume)
    {
        BGMVolumeMultiplier = Mathf.Clamp01(Volume);
        if (BgmSource != null && BGMFadeCoroutine == null)
        {
            BgmSource.volume = BGMVolumeMultiplier;
        }
    }
    public void SetSFXVolume(float Volume)
    {
        SFXVolumeMultiplier = Volume;
    }
    // ------------------------
    // BGM
    // ------------------------
    public void PlayBGM(int Index)
    {
        if (Index < 0 || Index >= BgmList.Count)
            return;

        PlayBGM(BgmList[Index]);
    }
    public void PlayBGM(AudioClip Clip)
    {
        if (Clip == null || BgmSource == null)
        {
            return;
        }

        if (BgmSource.clip == Clip)
        {
            BgmSource.loop = true;
            if (!BgmSource.isPlaying)
            {
                BgmSource.Play();
            }
            StartBGMFade(BgmSource.volume, BGMVolumeMultiplier, BGMFadeInDuration, false);
            return;
        }

        StopBGMFade();
        BgmSource.volume = 0.0f;
        BgmSource.clip = Clip;
        BgmSource.loop = true;
        BgmSource.Play();
        StartBGMFade(0.0f, BGMVolumeMultiplier, BGMFadeInDuration, false);
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
        if (Clip == null)
        {
            return;
        }
        SFXSource.volume = SFXVolumeMultiplier;
        SFXSource.PlayOneShot(Clip);
    }
    public void PlaySFX(AudioClip Clip, float VolumeBase)
    {
        if (Clip == null || SFXSource == null)
        {
            return;
        }
        SFXSource.volume = Mathf.Clamp01(VolumeBase * SFXVolumeMultiplier);
        SFXSource.PlayOneShot(Clip);
    }
    private void PlayButtonClick()
    {
        PlaySFX(ButtonClickSFX);
    }
}
