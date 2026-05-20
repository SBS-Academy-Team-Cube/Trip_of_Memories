using UnityEngine;

[CreateAssetMenu(menuName = "Sound/Random Sound Queue Data")]
public class RandomSoundQueue : ScriptableObject
{
    [SerializeField] private AudioClip[] Clips;
    [SerializeField] private bool WithoutReplacement = false;
    private int[] shuffledIndices;
    private int currentIndex;
    private void OnEnable()
    {
        InitializeShuffleBag();
    }
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
        if (!WithoutReplacement)
        {
            return Clips[Random.Range(0, Clips.Length)];
        }
        if (currentIndex >= shuffledIndices.Length)
        {
            Shuffle();
            currentIndex = 0;
        }

        return Clips[shuffledIndices[currentIndex++]];
    }

    private void InitializeShuffleBag()
    {
        if (Empty())
        {
            return;
        }
        shuffledIndices = new int[Clips.Length];
        for (int i = 0; i < Clips.Length; i++)
        {
            shuffledIndices[i] = i;
        }
        Shuffle();
        currentIndex = 0;
    }
    private void Shuffle()
    {
        for (int i = shuffledIndices.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            (shuffledIndices[i], shuffledIndices[randomIndex]) =
                (shuffledIndices[randomIndex], shuffledIndices[i]);
        }
    }
}