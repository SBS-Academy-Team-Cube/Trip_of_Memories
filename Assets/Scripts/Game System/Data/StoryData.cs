using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Dialogue/Story Data")]
public class StoryData : ScriptableObject
{

    public List<FStoryContext> Data;
    public FStoryContext Get(int Index)
    {
        if (Index < 0 || Index >= Data.Count)
        {
            return new FStoryContext(string.Empty);
        }

        FStoryContext context = Data[Index];

        if (string.IsNullOrWhiteSpace(context.Speaker))
        {
            context.Speaker = "???";
        }
        return context;
    }
    public int Count => Data.Count;
}

[Serializable]
public struct FStoryContext
{
    public string Speaker;
    [TextArea(3, 10)]
    public string Text;
    public bool NoEffect;
    public AudioClip SFX;
    public FStoryContext(string Text, string Speaker = "???", bool NoEffect = true, AudioClip SFX = null)
    {
        this.Speaker = Speaker;
        this.Text = Text;
        this.NoEffect = NoEffect;
        this.SFX = SFX;
    }
}