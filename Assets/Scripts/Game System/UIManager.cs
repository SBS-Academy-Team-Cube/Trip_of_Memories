using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenuPanel;

    public void ShowMainMenu(bool bShowing)
    {
        MainMenuPanel.SetActive(bShowing);
    }
}
