using UnityEngine;

[CreateAssetMenu(menuName = "Sound/Random Sound Queue Data")]
public class RandomSoundQueue : ScriptableObject
{
    [SerializeField] private AudioClip[] Clips;

    public bool Empty()
    {
        return Clips == null || Clips.Length == 0;
    }

    public AudioClip GetSound()
    {
        if (Empty())
        {
            return null;
        }
        return Clips[Random.Range(0, Clips.Length)];
    }
}
