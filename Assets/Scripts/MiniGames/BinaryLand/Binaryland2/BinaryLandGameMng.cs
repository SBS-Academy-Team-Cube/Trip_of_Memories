using UnityEngine;

public class BinaryLandGameMng : MonoBehaviour
{
    [SerializeField] private BinaryInputMng inputMng;
    [SerializeField] private BinaryUIMng UImng;
    [SerializeField] private BinaryPlayer[] players;
    [SerializeField] private BinaryLandMapEditor editor;

    private void Start() // test
    {
        inputMng.GameStart();
        //UImng.GameStart();
        foreach (var player in players)
        {
            player.GameStart();
        }
        editor.GameStart(0);
    }
    public void GameStart()
    {
        inputMng.GameStart();
        //UImng.GameStart();
        foreach (var player in players)
        {
            player.GameStart();
        }
        editor.GameStart(0);
    }
    public void GameStop()
    {

    }
    private void GameClear()
    {
        Debug.Log("Clear");
        UImng.GameClear();
        inputMng.GameExit();
    }
    public void MapClear(int mapIndex)
    {
        if(mapIndex == 2)
            GameClear();
        Debug.Log($"Clear {mapIndex}");
        editor.GameStart(mapIndex + 1);
        foreach(var player in players)
        {
            player.GameStart();
        }
    }


}
