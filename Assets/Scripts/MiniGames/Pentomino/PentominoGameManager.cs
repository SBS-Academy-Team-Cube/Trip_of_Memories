using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PentominoGameManager : MiniGameBase
{
    [SerializeField] private PentominoInputHandler inputHandler;
    [SerializeField] private PentominoBoard board;
    [SerializeField] private PentominoPiece[] pieces;

    public override event Action OnPlay;
    public override event Action OnClear;
    public override event Action OnFail;



    [SerializeField] private Trigger GameStartTrigger;
    [SerializeField] private Trigger GameClearTrigger;

    private PlayerInput PlayerInputSystem = null;
    private bool IsTriggered = false;
    private void OnEnable()
    {
        EventBus.PentominoClear += Clear;
    }
    private void OnDisable()
    {
        EventBus.PentominoClear -= Clear;

    }
    public override void Play()
    {
        Init();
        OnPlay?.Invoke();
    }
    public override void Clear()
    {
        if (GameClearTrigger != null)
        {
            GameClearTrigger.OnTrigger();
        }
        OnClear?.Invoke();
    }
    public override void Fail()
    {
        OnFail?.Invoke();
    }
    public override bool HasCleared()
    {
        if (SaveManager.Instance)
        {
            return SaveManager.Instance.HasClearedMiniGame(MiniGameID);
        }
        return false;
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
        if (IsTriggered || HasCleared())
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
            Play();
        }
    }
}
