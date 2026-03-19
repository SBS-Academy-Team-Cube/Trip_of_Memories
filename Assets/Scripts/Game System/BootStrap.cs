using UnityEngine;

public class BootStrap : MonoBehaviour
{
    void Start()
    {
        SaveManager.Instance.Load();
        var Data = SaveManager.Instance.Data;

        if(!Data.HasPlayed)
        {
            GameDirector.Instance.LoadScene("CharacterSelectScene");
        }
        else
        {
            GameDirector.Instance.LoadScene("MainMenuScene");
        }
    }

}
