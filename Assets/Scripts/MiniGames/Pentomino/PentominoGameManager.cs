using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PentominoGameManager : MiniGameBase
{
    [SerializeField] private PentominoInputHandler InputHandler;
    [SerializeField] private PentominoBoard Board;
    [SerializeField] private PentominoPiece[] Pieces;

    [SerializeField] private GameObject MiniGameRule;

    public override event Action OnPlay;
    public override event Action OnClear;
    public override event Action OnFail;

    [SerializeField] private Trigger GameStartTrigger;
    [SerializeField] private Trigger GameClearTrigger;

    private PlayerInput PlayerInputSystem = null;
    private bool IsTriggered = false;

    private void OnEnable()
    {
        Board.OnClear += Clear;
        InputHandler.OnQuitGame += Fail;
    }
    private void OnDisable()
    {
        Board.OnClear -= Clear;
        InputHandler.OnQuitGame -= Fail;
    }
    public override void Play()
    {
        if (GameDirector.Instance != null)
        {
            GameDirector.Instance.ShowMouseCursor(true);
            GameDirector.Instance.EnablePauseAction(false);
        }
        if (GameStartTrigger != null)
        {
            GameStartTrigger.OnTrigger();
        }

        if (MiniGameRule)
        {
            MiniGameRule.SetActive(true);
        }

        Board.Init();
        InputHandler.Init();
        foreach (var piece in Pieces)
        {
            piece.Init();
        }
        OnPlay?.Invoke();
    }
    public override void Clear()
    {
        if (GameStartTrigger != null)
        {
            GameStartTrigger.OnTrigger();
        }
        if (GameClearTrigger != null)
        {
            GameClearTrigger.OnTrigger();
        }
        if (GameDirector.Instance != null)
        {
            GameDirector.Instance.ShowMouseCursor(false);
            GameDirector.Instance.EnablePauseAction(true);
        }
        if (PlayerInputSystem != null)
        {
            PlayerInputSystem.enabled = true;
        }

        if (MiniGameRule)
        {
            MiniGameRule.SetActive(false);
        }

        OnClear?.Invoke();
    }
    public override void Fail()
    {
        if (GameStartTrigger != null)
        {
            GameStartTrigger.OnTrigger();
        }
        if (PlayerInputSystem != null)
        {
            PlayerInputSystem.enabled = true;
        }
        if (GameDirector.Instance != null)
        {
            GameDirector.Instance.ShowMouseCursor(true);
            GameDirector.Instance.EnablePauseAction(false);
        }

        if (MiniGameRule)
        {
            MiniGameRule.SetActive(false);
        }

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
            IsTriggered = true;
            Play();
        }
    }
    void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag("Player") && IsTriggered && !HasCleared())
        {
            IsTriggered = false;
        }
    }
}
