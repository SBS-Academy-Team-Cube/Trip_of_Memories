using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayableDirector director;
    private PlayerMovement PlayerMove;
    [SerializeField] Collider TriggerZone;
    private bool isPlaying = false;
    private bool Triggered = false;
    public void SetPlayerMovement(PlayerMovement PlayerMove)
    {
        this.PlayerMove = PlayerMove;
    }
    void OnTriggerEnter(Collider Other)
    {
        if (Triggered)
        {
            return;
        }
        if (Other.CompareTag("Player"))
        {
            Triggered = true;
            Play();
        }
    }
    private void Awake()
    {
        if (director != null)
        {
            director.stopped += OnCutsceneEnd;
        }
    }
    public void Play()
    {
        if (isPlaying)
        {
            return;
        }
        isPlaying = true;
        LockPlayer();
        director.Play();
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        EndCutscene();
    }

    private void EndCutscene()
    {
        UnlockPlayer();
        isPlaying = false;
    }

    private void LockPlayer()
    {
        if (PlayerMove != null)
        {
            PlayerMove.enabled = false;
        }
    }
    private void UnlockPlayer()
    {
        if (PlayerMove != null)
        {
            PlayerMove.enabled = true;
        }
    }
}