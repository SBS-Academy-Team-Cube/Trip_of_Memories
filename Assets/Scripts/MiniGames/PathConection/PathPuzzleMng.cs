using UnityEngine;

public class PathPuzzleMng : MonoBehaviour
{
    [Header("Puzzle Answer")]
    [SerializeField] private int greenAnswer = 0;
    [SerializeField] private int blueAnswer = 0;
    [SerializeField] private int redAnswer = 0;

    [SerializeField] private PathInputHandler inputHandler;
    [SerializeField] private PathPuzzleUIMng PathPuzzleUIMng;


    public void GameStart()
    {
        if (inputHandler == null)
            Debug.LogError("inputMng is null");
        if (PathPuzzleUIMng == null)
            Debug.LogError("UIMng is null");

        inputHandler.GameStart();
        PathPuzzleUIMng.GameStart();
    }
    public void GameExit()
    {
        PathPuzzleUIMng.GameStop();
    }

    private void Clear()
    {
        PathPuzzleUIMng.GameStop();
        Debug.Log("Clear");
    }


    private void OnEnable()
    {
        PathPuzzleUIMng.ClearEvent.AddListener(ClearCheck);
    }
    private void OnDisable()
    {
        PathPuzzleUIMng.ClearEvent.RemoveListener(ClearCheck);
    }

    private void ClearCheck()
    {
        if(greenAnswer == (PathPuzzleUIMng.GreenRollCount % 4) &&
            blueAnswer == (PathPuzzleUIMng.BlueRollCount % 4) &&
            redAnswer == (PathPuzzleUIMng.RedRollCount % 4))
        {
            Clear();
        }
    }


}
