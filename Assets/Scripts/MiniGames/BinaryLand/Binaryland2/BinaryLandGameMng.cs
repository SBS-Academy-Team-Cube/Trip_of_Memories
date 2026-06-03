using UnityEngine;

public class BinaryLandGameMng : MonoBehaviour
{
    [SerializeField] private BinaryInputMng inputMng;
    [SerializeField] private BinaryUIMng UImng;
    [SerializeField] private BinaryPlayer[] players;

    private void Start() // test
    {
        inputMng.GameStart();
        //UImng.GameStart();
        foreach (var player in players)
        {
            player.GameStart();
        }
    }
    public void GameStart()
    {
        inputMng.GameStart();
        //UImng.GameStart();
        foreach (var player in players)
        {
            player.GameStart();
        }
    }
    public void GameStop()
    {

    }
    public void GameClear()
    {

    }


}
