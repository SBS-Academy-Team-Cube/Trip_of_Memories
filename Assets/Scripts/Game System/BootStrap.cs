using UnityEngine;

public class BootStrap : MonoBehaviour
{
    void Start()
    {
        SaveManager.Instance.Load();
        var Data = SaveManager.Instance.Data;

        if (!Data.HasPlayed)
        {
            GameDirector.Instance.LoadSceneWithoutLoading(SceneId.CharacterSelect);
        }
        else
        {
            GameDirector.Instance.LoadSceneWithoutLoading(SceneId.MainMenu);
        }
    }
}
