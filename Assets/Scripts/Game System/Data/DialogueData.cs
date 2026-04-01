using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [TextArea(3, 10)]
    public List<string> Dialogues;

    public string GetText(int Index) 
    {
        if(Index < 0 || Index >= Dialogues.Count)
        {
            Debug.LogWarning("Invalid Index");
            return string.Empty;
        }   
        return Dialogues[Index];
    }
}