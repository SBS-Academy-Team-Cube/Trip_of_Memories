using UnityEngine;
using TMPro;
public class MiniGameRule : MonoBehaviour
{
    [Header("How to play")]
    [SerializeField] private GameObject HowToPanel;
    [SerializeField] private TextMeshProUGUI HowToText;
    [SerializeField] private DialogueData Dialogue;
    public int Index = 0;
    public void OnButtonClicked()
    {
        UIEventBus.OnAnyButtonClicked?.Invoke();
    }
    public void HowToReset()
    {
        if (HowToText == null || Dialogue == null)
        {
            return;
        }
        Index = 0;
        HowToText.text = Dialogue.GetText(Index);
    }
    public void Next(int AddIndex)
    {
        Index += AddIndex;
        if (Index < 0)
        {
            Index = 0;
        }
        else if (Index >= Dialogue.Dialogues.Count)
        {
            Index = Dialogue.Dialogues.Count - 1;
        }
        else
        {
            HowToText.text = Dialogue.GetText(Index);
        }
    }
}