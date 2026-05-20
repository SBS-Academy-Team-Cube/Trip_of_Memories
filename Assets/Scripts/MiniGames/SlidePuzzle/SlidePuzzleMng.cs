using UnityEngine;

public class SlidePuzzleMng : MonoBehaviour
{
    [SerializeField] private SlidePuzzleBoard board;
    [SerializeField] private SlidePuzzleInputHandler inputHandler;
    [SerializeField] private SlidePuzzleUIMng UIMng;//
    [SerializeField] private SlidePuzzlePiece[] pieces;


    private void Start()
    {
        GameStart();
    }
    public void GameStart() // game start trigger
    {
        board.GameInit();
        PiecesInit();
        UIMng.GameInit(board);
    }
    public void GameReset() // game reset trigger
    {
        board.GameInit();
        PiecesInit();
        UIMng.GameInit(board);
    }
    public void GameClear() // game end trigger
    {
        UIMng.GameClear();
    }



    private void PiecesInit()
    {
        foreach (var piece in pieces)
        {
            piece.GameInit();
        }
    }
}
