using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text TextUI;
    [SerializeField] private AudioSource AudioSource;
    private Coroutine TypingCoroutine;
    public bool IsTyping = false;
    public void Play(string FullText, TypewriterEffect Effect)
    {
        if (TypingCoroutine != null)
            StopCoroutine(TypingCoroutine);

        TypingCoroutine = StartCoroutine(TypeRoutine(FullText, Effect));
    }
    public void Skip()
    {
        if (!IsTyping)
        {
            return;
        }

        StopCoroutine(TypingCoroutine);
        TypingCoroutine = null;
        IsTyping = false;
    }
    public void Skip(string FullText)
    {
        if (TypingCoroutine != null)
            StopCoroutine(TypingCoroutine);

        TextUI.text = FullText;
        IsTyping = false;
    }
    private IEnumerator TypeRoutine(string FullText, TypewriterEffect Effect)
    {
        IsTyping = true;
        TextUI.text = "";
        int SoundCounter = 0;

        for (int i = 0; i < FullText.Length; i++)
        {
            TextUI.text += FullText[i];
            if (Effect.TypingSound && AudioSource)
            {
                SoundCounter++;
                if (SoundCounter * Effect.Delay >= Effect.SoundInterval)
                {
                    AudioSource.PlayOneShot(Effect.TypingSound);
                    SoundCounter = 0;
                }
            }
            float Wait = Effect.Delay;
            if (Effect.UseUnscaledTime)
                yield return new WaitForSecondsRealtime(Wait);
            else
                yield return new WaitForSeconds(Wait);
        }
        IsTyping = false;
    }
}