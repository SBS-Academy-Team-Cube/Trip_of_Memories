using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Image BackgroundImg;
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private Button ContinueBtn;

    private void Start()
    {
        if (GameDirector.Instance)
        {
            GameDirector.Instance.OnPaused += HandlePause;
            ContinueBtn.onClick.AddListener(GameDirector.Instance.ContinueGame);
        }
        HandlePause(false);
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        if (GameDirector.Instance)
        {
            GameDirector.Instance.OnPaused -= HandlePause;
            ContinueBtn.onClick.RemoveListener(GameDirector.Instance.ContinueGame);
        }
    }
    private void HandlePause(bool bPaused)
    {
        BackgroundImg.enabled = bPaused;
        PauseMenuUI.SetActive(bPaused);
    }
}
