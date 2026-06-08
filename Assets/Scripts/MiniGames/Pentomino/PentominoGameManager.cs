using UnityEngine;
using UnityEngine.InputSystem;

public class PentominoGameManager : MonoBehaviour
{
    [SerializeField] private PentominoInputHandler inputHandler;
    [SerializeField] private PentominoBoard board;
    [SerializeField] private PentominoPiece[] pieces;

    [SerializeField] private Trigger GameStartTrigger;
    [SerializeField] private Trigger GameClearTrigger;

    private PlayerInput PlayerInputSystem = null;
    private bool IsTriggered = false;
    private void OnEnable()
    {
        EventBus.PentominoClear += GameClear;
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
        foreach (var piece in pieces)
        {
            piece.Init();
        }
    }
    void OnTriggerEnter(Collider Other)
    {
        if (IsTriggered)
        {
            return;
        }
        if (Other.CompareTag("Player"))
        {
            if (Other.TryGetComponent(out PlayerInputSystem))
            {
                PlayerInputSystem.enabled = false;
            }

            if (GameDirector.Instance != null)
            {
                GameDirector.Instance.ShowMouseCursor(true);
            }

            IsTriggered = true;
            if (GameStartTrigger != null)
            {
                GameStartTrigger.OnTrigger();
            }
            GameStart();
        }
    }
    public void GameClear()
    {
        if (GameClearTrigger != null)
        {
            GameClearTrigger.OnTrigger();
        }
    }
}
