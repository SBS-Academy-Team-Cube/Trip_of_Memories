using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class BucketUIManager : MonoBehaviour
{
    [Header("How to play")]
    [SerializeField] private GameObject HowToPanel;
    [SerializeField] private TextMeshProUGUI HowToText;
    [SerializeField] private DialogueData Dialogue;
    [Header("PopUp")]
    [SerializeField] private GameObject PopupPanel;
    [SerializeField] private TextMeshProUGUI PopupText;

    public int Index = 0;
    public void ShowPopup(string Text)
    {
        StopAllCoroutines();
        PopupText.text = Text;
        StartCoroutine(ShowingPopupRoutine());
    }
    private IEnumerator ShowingPopupRoutine()
    {
        PopupPanel.SetActive(true);
        yield return new WaitForSeconds(.5f);
        PopupPanel.SetActive(false);
    }
    public void OnButtonClicked()
    {
        UIEventBus.OnAnyButtonClicked?.Invoke();
    }
    public void bShowingHowToPanel(bool bShowing)
    {
        if (bShowing)
        {
            HowToText.text = Dialogue.GetText(Index);
            HowToPanel.SetActive(true);
        }
        else
        {
            HowToPanel.SetActive(false);
            Index = 0;
        }
    }
    public void Next(int AddIndex)
    {
        Index += AddIndex;
        if (Index < 0)
        {
            Index = 0;
        }
        else if (Index > Dialogue.Dialogues.Count)
        {
            Index = Dialogue.Dialogues.Count - 1;
        }
        else
        {
            HowToText.text = Dialogue.GetText(Index);
        }
    }
}
