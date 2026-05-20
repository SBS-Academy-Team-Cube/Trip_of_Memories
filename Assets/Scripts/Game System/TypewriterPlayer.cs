using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text TextUI;
    [SerializeField] private AudioSource AudioSource;
    private Coroutine TypingCoroutine;
    public bool IsTyping { get; private set; }
    public System.Action OnTypingFinished;
    public void Play(string FullText, TypewriterEffect Effect)
    {
        if (TypingCoroutine != null)
        {
            StopCoroutine(TypingCoroutine);
        }
        TypingCoroutine = StartCoroutine(TypeRoutine(FullText, Effect));
    }
    public void Skip(string FullText)
    {
        if (TypingCoroutine != null)
        {
            StopCoroutine(TypingCoroutine);
        }
        TextUI.text = FullText;
        IsTyping = false;
        OnTypingFinished?.Invoke();
    }
    private IEnumerator TypeRoutine(string FullText, TypewriterEffect Effect)
    {
        IsTyping = true;
        TextUI.text = "";
        if (AudioSource != null && !Effect.TypingSounds.Empty())
        {
            StartCoroutine(PlayRandomSound(Effect));
        }
        foreach (char Character in FullText)
        {
            TextUI.text += Character;
            yield return new WaitForSeconds(Effect.Delay);
        }
        IsTyping = false;
        TypingCoroutine = null;
        OnTypingFinished?.Invoke();
    }

    private IEnumerator PlayRandomSound(TypewriterEffect Effect)
    {
        while (IsTyping)
        {
            AudioSource.pitch = Random.Range(0.9f, 1.35f);
            AudioClip RandomClip = Effect.TypingSounds.GetSound();
            AudioSource.PlayOneShot(RandomClip);
            yield return new WaitForSeconds(RandomClip.length / AudioSource.pitch);
        }
    }
}