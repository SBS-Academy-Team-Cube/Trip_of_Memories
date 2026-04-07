using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private List<AudioClip> BgmList;

    [Header("SFX")]
    [SerializeField] private AudioSource SfxSource;
    [SerializeField] private AudioClip ButtonClickSFX;

    protected override void Awake()
    {
        base.Awake();
        UIEventBus.OnAnyButtonClicked += PlayButtonClick;
    }
    private void OnDestroy()
    {
        UIEventBus.OnAnyButtonClicked -= PlayButtonClick;
    }

    // ------------------------
    // BGM
    // ------------------------
    public void PlayBGM(int Index)
    {
        if (Index < 0 || Index >= BgmList.Count)
            return;

        BgmSource.clip = BgmList[Index];
        BgmSource.loop = true;
        BgmSource.Play();
    }
    public void StopBGM()
    {
        BgmSource.Stop();
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
        SfxSource.PlayOneShot(Clip);
    }
    private void PlayButtonClick()
    {
        PlaySFX(ButtonClickSFX);
    }
}