using UnityEngine;

public class PentominoGameManager : MonoBehaviour
{
    [SerializeField] private PentominoInputHandler inputHandler;
    [SerializeField] private PentominoBoard board;
    [SerializeField] private PentominoPiece[] pieces;

    private void OnEnable()
    {
        EventBus.PentominoClear += GameClear;
        GameStart();
    }
    private void OnDisable()
    {
        EventBus.PentominoClear -= GameClear;
    }
    private void GameStart()
    {
        Init();
    }
    private void GameReset()
    {

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
    private void GameClear()
    {
        Debug.Log("Game Clear");
    }
}
