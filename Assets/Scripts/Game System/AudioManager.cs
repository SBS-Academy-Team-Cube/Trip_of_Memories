using System.Collections;
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

        BgmSource.volume = 1.0f;
        BgmSource.clip = BgmList[Index];
        BgmSource.loop = true;
        BgmSource.Play();
    }
    public void PlayBGM(AudioClip Clip)
    {
        BgmSource.volume = 1.0f;
        BgmSource.clip = Clip;
        BgmSource.loop = true;
        BgmSource.Play();
    }
    public void StopBGM()
    {
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        float Timer = 0.0f;
        while (Timer < 1.5f)
        {
            Timer += Time.deltaTime;
            BgmSource.volume = Mathf.Lerp(0.0f, 1.5f, Timer / 1.5f);
            yield return null;
        }
        BgmSource.volume = 0.0f;
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