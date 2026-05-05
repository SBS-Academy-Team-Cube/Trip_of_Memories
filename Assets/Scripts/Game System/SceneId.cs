public enum SceneId
{
    Boot,
    CharacterSelect,
    LevelSelect,
    MainMenu,
    Loading,
    Stage1_1,
    Stage1_2,
    Stage1_3,
    Stage1_4,
    NULL,
}
public static class SceneTable
{
    public static string GetSceneName(SceneId sceneId)
    {
        return sceneId switch
        {
            SceneId.Boot => "BootScene",
            SceneId.CharacterSelect => "CharacterSelectScene",
            SceneId.MainMenu => "MainMenuScene",
            SceneId.Loading => "LoadingScene",
            SceneId.Stage1_1 => "Stage 1-1",
            SceneId.Stage1_2 => "Stage 1-2",
            SceneId.Stage1_3 => "Stage 1-3",
            SceneId.Stage1_4 => "Stage 1-4",
            // _ => "MainMenuScene",
        };
    }

    public static GameState GetGameState(SceneId sceneId)
    {
        return sceneId switch
        {
            SceneId.CharacterSelect => GameState.CharacterSelect,
            SceneId.Loading => GameState.Loading,
            SceneId.Stage1_1 or SceneId.Stage1_2 or SceneId.Stage1_3 or SceneId.Stage1_4 => GameState.InGame,
            _ => GameState.None,
        };
    }
}
