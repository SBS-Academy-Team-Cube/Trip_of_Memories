using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Image BackgroundImg;
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private Button ContinueBtn;
    private GameDirector Director;

    private void OnEnable()
    {
        Director = GameDirector.Instance;
        if (Director)
        {
            Director.OnPaused += HandlePause;
            ContinueBtn.onClick.AddListener(Director.ContinueGame);
            HandlePause(Director.IsPaused);
            return;
        }
        
        HandlePause(false);
    }
    private void OnDisable()
    {
        if (Director)
        {
            Director.OnPaused -= HandlePause;
            ContinueBtn.onClick.RemoveListener(Director.ContinueGame);
        }

        Director = null;
    }
    private void HandlePause(bool bPaused)
    {
        BackgroundImg.enabled = bPaused;
        PauseMenuUI.SetActive(bPaused);
    }
}
