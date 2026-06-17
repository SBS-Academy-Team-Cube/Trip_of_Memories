using UnityEngine;
using TMPro;
using System.Collections;

public class UIPopUp : MonoBehaviour
{
    [SerializeField] private GameObject PopupPanel;
    [SerializeField] private TextMeshProUGUI PopupText;
    public void ShowPopup(string Text, float Duration = 1.0f)
    {
        StopAllCoroutines();
        PopupText.text = Text;
        StartCoroutine(ShowingPopupRoutine(Duration));
    }
    private IEnumerator ShowingPopupRoutine(float Duration)
    {
        PopupPanel.SetActive(true);
        yield return new WaitForSeconds(Duration);
        PopupPanel.SetActive(false);
    }
    public void OnButtonClicked()
    {
        UIEventBus.OnAnyButtonClicked?.Invoke();
    }
}
