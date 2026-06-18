using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text TextUI;
    private Coroutine TypingCoroutine;
    public System.Action OnTypingFinished;
    public void Play(string FullText, TypewriterEffect Effect)
    {
        if (TypingCoroutine != null)
        {
            StopCoroutine(TypingCoroutine);
        }
        TypingCoroutine = StartCoroutine(TypeRoutine(FullText, Effect));
    }
    public void Play(string FullText)
    {
        TextUI.text = FullText;
        OnTypingFinished?.Invoke();
    }
    public void Skip(string FullText)
    {
        if (TypingCoroutine != null)
        {
            StopCoroutine(TypingCoroutine);
        }
        TextUI.text = FullText;
        OnTypingFinished?.Invoke();
    }
    private IEnumerator TypeRoutine(string FullText, TypewriterEffect Effect)
    {
        TextUI.text = "";
        int SoundChar = 2;
        int SoundIdx = 0;
        foreach (char Character in FullText)
        {
            TextUI.text += Character;
            SoundIdx++;
            if (SoundIdx == SoundChar && !Effect.TypingSounds.Empty() && AudioManager.Instance != null)
            {
                SoundIdx = 0;
                AudioManager.Instance.PlaySFX(Effect.TypingSounds.GetSound());
            }
            yield return new WaitForSeconds(Effect.Delay);
        }
        OnTypingFinished?.Invoke();
    }
}