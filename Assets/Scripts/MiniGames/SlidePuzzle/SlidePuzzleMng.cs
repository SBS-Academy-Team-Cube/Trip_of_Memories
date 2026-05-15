using UnityEngine;

public class SlidePuzzleMng : MonoBehaviour
{
    [SerializeField] private SlidePuzzleUIMng UIMng;
    [SerializeField] private SlidePuzzleInputHandler InputHandler;
    [SerializeField] private SlidePuzzleBoard board;

    private void Start() // testcode
    {
        GameStart();
    }



    public void GameStart()
    {
        if (InputHandler == null)
            return;
        if (board == null)
            return;
        if (UIMng == null)
            return;


        InputHandler.GameInit();
        board.GameInit();
        UIMng.GameInit();
        UIMng.GameStart();
    }
    public void GameReset()
    {
        UIMng.GameInit();
    }
    public void GameClear()
    {
        UIMng.GameClear();
    }
}
