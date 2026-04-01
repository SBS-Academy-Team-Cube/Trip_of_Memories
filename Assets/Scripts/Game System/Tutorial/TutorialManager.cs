using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialText Texts;
    [SerializeField] private List<TutorialTriggerZone> Zones;
    [SerializeField] private float Duration = 10.0f;
    [SerializeField] private GameObject TextPanel;
    [SerializeField] private TMP_Text Text;
    [SerializeField] private InputActionReference SkipAction;
    [SerializeField] private InputActionReference PlayerJumpAction;

    private Coroutine CurrentRoutine = null;

    public void ShowTutorialText(int Index)
    {
        PlayerJumpAction.action.Disable();
        SkipAction.action.Enable();

        Text.SetText(Texts.GetText(Index));
        TextPanel.SetActive(true);
        CurrentRoutine = StartCoroutine(VisibleDuration());
    }
    private IEnumerator VisibleDuration()
    {
        yield return new WaitForSeconds(Duration);
        PlayerJumpAction.action.Enable();
        SkipAction.action.Disable();
        TextPanel.SetActive(false);
    }
    private void OnEnable()
    {
        SkipAction.action.performed += OnSkip;
        if (TextPanel != null)
        {
            TextPanel.SetActive(false);
        }
        for (int i = 0; i < Zones.Count; i++)
        {
            Zones[i].Init(this, i);
        }
    }
    private void OnDisable()
    {
        SkipAction.action.performed -= OnSkip;
    }

    void OnSkip(InputAction.CallbackContext Context)
    {
        if (CurrentRoutine != null)
        {
            StopCoroutine(CurrentRoutine);
            TextPanel.SetActive(false);
            PlayerJumpAction.action.Enable();
            SkipAction.action.Disable();
        }
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