using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public TutorialText Texts;
    [SerializeField] private float Duration = 10.0f;
    [SerializeField] private GameObject TextPanel;
    [SerializeField] private TMP_Text Text;

    public void ShowTutorialText(int Index)
    {
        Text.SetText(Texts.GetText(Index));
        TextPanel.SetActive(true);
        StartCoroutine(VisibleDuration());
    }
    private IEnumerator VisibleDuration()
    {
        yield return new WaitForSeconds(Duration);
        TextPanel.SetActive(false);
    }

    private void OnEnable() 
    {
        TutorialTriggerZone.OnTriggered += ShowTutorialText;
    }
    private void OnDisable() 
    {
        TutorialTriggerZone.OnTriggered -= ShowTutorialText;
    }

}

[CreateAssetMenu(fileName = "TutorialText", menuName = "TutorialText", order = 0)]
public class TutorialText : ScriptableObject 
{
    public List<string> Texts;
    public string GetText(int idx)
    {
        if (idx < 0 || idx >= Texts.Count)
        {
            Debug.LogWarning("Invalid Index");
            return string.Empty;
        }

        return Texts[idx];
    }
}