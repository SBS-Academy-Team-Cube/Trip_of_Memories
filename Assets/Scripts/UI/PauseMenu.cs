using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Image BackgroundImg;
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private Button ContinueBtn;
    [SerializeField] private Button ToMainMenuBtn;
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
            Debug.Log("GameDirector in Scene");
            return;
        }
        else
        {
            Debug.Log("No GameDirector in Scene");
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
        BackgroundImg.enabled = bPaused;
        PauseMenuUI.SetActive(bPaused);
    }
    private void ToMainMenuRoutine()
    {
        Director.ContinueGame();
        HandlePause(false);
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
}
