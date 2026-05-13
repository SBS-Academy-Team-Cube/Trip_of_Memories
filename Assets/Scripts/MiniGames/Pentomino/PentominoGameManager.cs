using UnityEngine;

public class PentominoGameManager : MonoBehaviour
{
    [SerializeField] private PentominoInputHandler inputHandler;
    [SerializeField] private PentominoBoard board;
    [SerializeField] private PentominoPiece[] pieces;

    private void OnEnable()
    {
        EventBus.PentominoClear += GameClear;
        GameStart();// trigger
    }
    private void OnDisable()
    {
        EventBus.PentominoClear -= GameClear;
    }
    public void GameStart()//
    {
        Init();
    }
    public void GameReset()//
    {
        Init();
    }
    private void Init()
    {
        board.Init();
        inputHandler.Init();
        foreach(var piece in pieces)
        {
            piece.Init();
        }
    }
    public void GameClear()//
    {
        Debug.Log("Game Clear");
    }
}
