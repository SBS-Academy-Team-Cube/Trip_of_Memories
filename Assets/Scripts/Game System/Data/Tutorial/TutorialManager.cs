using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private DialogueData TutorialDialogues;
    [SerializeField] private float Duration = 10.0f;
    [SerializeField] private GameObject TextPanel;
    [SerializeField] private TMP_Text Text;
    [SerializeField] private InputActionReference SkipAction;
    [SerializeField] private InputActionReference PlayerJumpAction;
    private Coroutine CurrentRoutine = null;
    public void ShowTutorialText(int Index)
    {
        if (CurrentRoutine != null)
        {
            StopCoroutine(CurrentRoutine);
            CurrentRoutine = null;
        }

        PlayerJumpAction.action.Disable();
        SkipAction.action.Enable();

        Text.SetText(TutorialDialogues.GetText(Index));
        TextPanel.SetActive(true);

        CurrentRoutine = StartCoroutine(VisibleDuration(Duration));
    }
    private IEnumerator VisibleDuration(float Delay = 1.0f)
    {
        yield return new WaitForSeconds(Delay);

        PlayerJumpAction.action.Enable();
        SkipAction.action.Disable();
        TextPanel.SetActive(false);
        CurrentRoutine = null;
    }
    private void OnEnable()
    {
        SkipAction.action.performed += OnSkip;
        if (TextPanel != null)
        {
            TextPanel.SetActive(false);
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
            CurrentRoutine = StartCoroutine(VisibleDuration());
            PlayerJumpAction.action.Enable();
            SkipAction.action.Disable();
        }
    }
}