using UnityEngine;

public class PathPuzzleMng : MonoBehaviour
{
    [Header("Puzzle Answer")]
    [SerializeField] private int greenAnswer = 0;
    [SerializeField] private int blueAnswer = 0;
    [SerializeField] private int redAnswer = 0;

    [SerializeField] private PathInputHandler inputHandler;

    private void Awake()
    {
        if (inputHandler == null)
            Debug.LogError("inputhandler is null");
    }
    private void OnEnable()
    {
        inputHandler.ClearCheckEvent.AddListener(ClearCheck);
    }
    private void OnDisable()
    {
        inputHandler.ClearCheckEvent.RemoveListener(ClearCheck);
    }

    private void ClearCheck()
    {
        if(greenAnswer == (inputHandler.GreenRollCount % 4) &&
            blueAnswer == (inputHandler.BlueRollCount % 4) &&
            redAnswer == (inputHandler.RedRollCount % 4))
        {
            Clear();
        }
    }

    private void Clear()
    {
        Debug.Log("Clear");
    }

}
