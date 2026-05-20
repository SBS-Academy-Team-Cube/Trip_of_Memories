using UnityEngine;

[CreateAssetMenu(menuName = "UI/Typewriter Effect")]
public class TypewriterEffect : ScriptableObject
{
    [Header("Typing Settings")]
    public float Delay = 0.05f;

    [Header("Sound")]
    public RandomSoundQueue TypingSounds;
}