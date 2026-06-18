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
    private bool IsSkipSubscribed = false;
    public void ShowTutorialText(int Index)
    {
        if (CurrentRoutine != null)
        {
            StopCoroutine(CurrentRoutine);
            CurrentRoutine = null;
        }

        PlayerJumpAction.action.Disable();

        SubscribeSkip();
        Text.SetText(TutorialDialogues.GetText(Index));
        TextPanel.SetActive(true);

        CurrentRoutine = StartCoroutine(VisibleDuration(Duration));
    }
    private void SubscribeSkip()
    {
        if (IsSkipSubscribed || SkipAction == null)
        {
            return;
        }

        SkipAction.action.performed += OnSkip;
        IsSkipSubscribed = true;
    }
    private void UnsubscribeSkip()
    {
        if (!IsSkipSubscribed || SkipAction == null)
        {
            return;
        }

        SkipAction.action.performed -= OnSkip;
        IsSkipSubscribed = false;
    }
    private IEnumerator VisibleDuration(float Delay = 0.5f)
    {
        yield return new WaitForSeconds(Delay);
        UnsubscribeSkip();
        PlayerJumpAction.action.Enable();
        // SkipAction.action.Disable();
        TextPanel.SetActive(false);
        CurrentRoutine = null;
    }
    private void OnEnable()
    {
        SkipAction.action.Enable();
        // SkipAction.action.performed += OnSkip;
        if (TextPanel != null)
        {
            TextPanel.SetActive(false);
        }
    }
    private void OnDisable()
    {
        UnsubscribeSkip();
    }
    void OnSkip(InputAction.CallbackContext Context)
    {
        if (CurrentRoutine != null)
        {
            StopCoroutine(CurrentRoutine);
            CurrentRoutine = StartCoroutine(VisibleDuration());
            UnsubscribeSkip();
            PlayerJumpAction.action.Enable();

        }
    }
}