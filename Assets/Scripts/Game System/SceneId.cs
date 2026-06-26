using UnityEngine.SceneManagement;
public enum SceneId
{
    Boot,
    CharacterSelect,
    LevelSelect,
    MainMenu,
    Loading,
    Stage1_1, Stage1_2, Stage1_3, Stage1_4,
    Stage2_1, Stage2_2, Stage2_2_1, Stage2_3, Stage2_3_1, Stage2_4, Stage2_5,
    Stage3_1, Stage3_2, Stage3_3, Stage3_4,
    Stage4_1, Stage4_2, Stage4_3, Stage4_4, Stage4_5, Stage4_6,
    NULL,
}
public static class SceneTable
{
    public static string GetSceneName(SceneId sceneId)
    {
        return sceneId switch
        {
            SceneId.Boot => "BootScene",
            SceneId.CharacterSelect => "CharacterSelect",
            SceneId.MainMenu => "MainMenuScene",
            SceneId.Loading => "LoadingScene",
            SceneId.LevelSelect => "StageSelect",

            SceneId.Stage1_1 => "Stage 1-1",
            SceneId.Stage1_2 => "Stage 1-2",
            SceneId.Stage1_3 => "Stage 1-3",
            SceneId.Stage1_4 => "Stage 1-4",

            SceneId.Stage2_1 => "Stage 2-1",
            SceneId.Stage2_2 => "Stage 2-2",
            SceneId.Stage2_2_1 => "Stage 2-2-1",
            SceneId.Stage2_3 => "Stage 2-3",
            SceneId.Stage2_3_1 => "Stage 2-3-1",
            SceneId.Stage2_4 => "Stage 2-4",
            SceneId.Stage2_5 => "Stage 2-5",

            SceneId.Stage3_1 => "Stage 3-1",
            SceneId.Stage3_2 => "Stage 3-2",
            SceneId.Stage3_3 => "Stage 3-3",
            SceneId.Stage3_4 => "Stage 3-4",

            SceneId.Stage4_1 => "Stage 4-1",
            SceneId.Stage4_2 => "Stage 4-2",
            SceneId.Stage4_3 => "Stage 4-3",
            SceneId.Stage4_4 => "Stage 4-4",
            SceneId.Stage4_5 => "Stage 4-5",
            SceneId.Stage4_6 => "Stage 4-6",
            _ => "MainMenuScene",
        };
    }
    public static GameState GetGameState(SceneId sceneId)
    {
        return sceneId switch
        {
            SceneId.CharacterSelect => GameState.CharacterSelect,
            SceneId.Loading => GameState.Loading,
            SceneId.MainMenu => GameState.MainMenu,
            _ => GameState.InGame,
        };
    }
    public static bool IsLoadingScene(Scene scene)
    {
        return scene.name.Equals(GetSceneName(SceneId.Loading));
    }
}
