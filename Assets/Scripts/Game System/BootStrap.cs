using UnityEngine;

public class BootStrap : MonoBehaviour
{
    void Start()
    {
        SaveManager.Instance.Load();
        var Data = SaveManager.Instance.Data;
        
        if (!Data.HasPlayed)
        {
            GameDirector.Instance.LoadScene(SceneId.CharacterSelect);
        }
        else
        {
            GameDirector.Instance.LoadScene(SceneId.MainMenu);
        }
    }
}
