using UnityEngine;

public class PentominoGameManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus.PentominoClear += GameClear;
    }
    private void OnDisable()
    {
        EventBus.PentominoClear -= GameClear;
    }
    private void GameClear()
    {
        Debug.Log("Game Clear");
    }
}
