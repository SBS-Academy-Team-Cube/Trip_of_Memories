using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Image BackgroundImg;
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private GameObject VolumeOptionPanel;
    [SerializeField] private GameObject ButtonsPanel;
    [SerializeField] private Button ContinueBtn;
    [SerializeField] private Button ToMainMenuBtn;
    [SerializeField] private TMP_Text[] Texts;
    private GameDirector Director;
    private void OnEnable()
    {
        Director = GameDirector.Instance;
        if (Director)
        {
            Director.OnPaused += HandlePause;
            ContinueBtn.onClick.AddListener(Director.ContinueGame);
            ToMainMenuBtn.onClick.AddListener(ToMainMenuRoutine);

            HandlePause(Director.IsPaused);
            return;
        }
        for (int i = 0; i < Texts.Length; i++)
        {
            Texts[i].color = i == QualitySettings.GetQualityLevel() ? Color.red : Color.black;
        }
        HandlePause(false);
    }
    private void OnDisable()
    {
        if (Director)
        {
            Director.OnPaused -= HandlePause;
            ContinueBtn.onClick.RemoveListener(Director.ContinueGame);
            ToMainMenuBtn.onClick.RemoveAllListeners();
        }
        Director = null;
    }
    private void HandlePause(bool bPaused)
    {
        if (!bPaused)
        {
            VolumeOptionPanel?.SetActive(false);
            ButtonsPanel.SetActive(true);
        }
        BackgroundImg.enabled = bPaused;
        PauseMenuUI.SetActive(bPaused);
    }
    private void ToMainMenuRoutine()
    {
        Director.ContinueGame();
        HandlePause(false);
        if (SaveManager.Instance)
        {
            SaveManager.Instance.ClearCurrentLevelProgress();
        }
        StartCoroutine(ToMainMenu());
    }
    private IEnumerator ToMainMenu()
    {
        yield return new WaitForSeconds(GameDirector.Instance.Iris.FadeOut() + 0.1f);
        GameDirector.Instance.LoadScene(SceneId.MainMenu);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGM();
        }
    }
    public void OnButtonClicked()
    {
        UIEventBus.OnAnyButtonClicked?.Invoke();
    }
    public void SetQualityLevel(int Index)
    {
        for (int i = 0; i < Texts.Length; i++)
        {
            Texts[i].color = i == Index ? Color.red : Color.black;
        }
        QualitySettings.SetQualityLevel(Index, true);
    }
}
