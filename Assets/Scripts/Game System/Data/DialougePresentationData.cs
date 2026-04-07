using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/Presentation")]
public class DialoguePresentationData : ScriptableObject
{
    public List<BackgroundEvent> BackgroundEvents;
    public Sprite GetBackground(int dialogueIndex)
    {
        for (int i = 0; i < BackgroundEvents.Count; i++)
        {
            if (BackgroundEvents[i].DialogueIndex == dialogueIndex)
                return BackgroundEvents[i].Background;
        }
        return null;
    }
}

[System.Serializable]
public class BackgroundEvent
{
    public int DialogueIndex;
    public Sprite Background;
}