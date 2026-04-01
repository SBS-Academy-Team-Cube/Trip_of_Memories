using UnityEngine;

[CreateAssetMenu(menuName = "UI/Typewriter Effect")]
public class TypewriterEffect : ScriptableObject
{
    [Header("Typing Settings")]
    public float Delay = 0.05f;
    public bool UseUnscaledTime = true;

    [Header("Skip Settings")]
    public bool AllowSkip = true;

    [Header("Sound")]
    public AudioClip TypingSound;
    public float SoundInterval = 0.05f;

    [Header("Advanced")]
    public bool IgnoreRichText = true;
}