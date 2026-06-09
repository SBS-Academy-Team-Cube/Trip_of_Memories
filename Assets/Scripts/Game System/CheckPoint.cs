using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private const string EnabledEffectRootName = "CheckPoint Enabled Effect";
    private static event Action<SceneId, string> OnCheckpointChanged;

    [SerializeField] private CheckpointData Data;
    [SerializeField] private Transform EnabledEffectRoot;

    public Transform SpawnPosition;

    public string CheckpointID => Data.CheckpointID;
    public SceneId SceneID => Data.SceneID;

    private bool bEnabled = false;
    private bool bHasAppliedEffectState = false;
    private bool bEffectCached = false;
    private ParticleSystem[] EnabledEffectParticles = Array.Empty<ParticleSystem>();
    private AudioSource[] EnabledEffectAudioSources = Array.Empty<AudioSource>();

    private void Awake()
    {
        CacheEnabledEffect();
    }

    private void OnEnable()
    {
        OnCheckpointChanged += HandleCheckpointChanged;
        RefreshEnabledState();
    }

    private void OnDisable()
    {
        OnCheckpointChanged -= HandleCheckpointChanged;
    }

    private void Start()
    {
        RefreshEnabledState();
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (!Other.CompareTag("Player"))
        {
            return;
        }

        if (Data == null || SaveManager.Instance == null)
        {
            return;
        }

        SaveManager.Instance.SetCheckpoint(Data.SceneID, Data.CheckpointID);
        OnCheckpointChanged?.Invoke(Data.SceneID, Data.CheckpointID);
    }

    private void RefreshEnabledState()
    {
        ApplyEnabledState(IsCurrentCheckpoint(), false);
    }

    private void HandleCheckpointChanged(SceneId ChangedSceneID, string ChangedCheckpointID)
    {
        bool bIsChangedCheckpoint = Data != null
            && ChangedSceneID == Data.SceneID
            && string.Equals(ChangedCheckpointID, Data.CheckpointID, StringComparison.Ordinal);

        ApplyEnabledState(bIsChangedCheckpoint, bIsChangedCheckpoint);
    }

    private bool IsCurrentCheckpoint()
    {
        if (Data == null || SaveManager.Instance == null || SaveManager.Instance.CurrentLevelProgress == null)
        {
            return false;
        }

        LevelProgressData Progress = SaveManager.Instance.CurrentLevelProgress;
        if (!Progress.HasCheckpoint())
        {
            return false;
        }

        CheckpointData LastCheckpoint = Progress.LastCheckpoint;
        return LastCheckpoint.SceneID == Data.SceneID
            && string.Equals(LastCheckpoint.CheckpointID, Data.CheckpointID, StringComparison.Ordinal);
    }

    private void ApplyEnabledState(bool bNewEnabled, bool bForcePlay)
    {
        if (!bForcePlay && bHasAppliedEffectState && bEnabled == bNewEnabled)
        {
            return;
        }

        bEnabled = bNewEnabled;
        bHasAppliedEffectState = true;

        if (bEnabled)
        {
            PlayEnabledEffect(bForcePlay);
        }
        else
        {
            StopEnabledEffect();
        }
    }

    private void PlayEnabledEffect(bool bRestart)
    {
        CacheEnabledEffect();

        if (EnabledEffectRoot == null)
        {
            return;
        }

        EnabledEffectRoot.gameObject.SetActive(true);

        foreach (ParticleSystem Particle in EnabledEffectParticles)
        {
            if (Particle == null)
            {
                continue;
            }

            if (bRestart)
            {
                Particle.Clear(true);
            }

            Particle.Play(true);
        }

        foreach (AudioSource Audio in EnabledEffectAudioSources)
        {
            if (Audio == null)
            {
                continue;
            }

            if (bRestart)
            {
                Audio.Stop();
            }

            if (!Audio.isPlaying)
            {
                Audio.Play();
            }
        }
    }

    private void StopEnabledEffect()
    {
        CacheEnabledEffect();

        foreach (ParticleSystem Particle in EnabledEffectParticles)
        {
            if (Particle == null)
            {
                continue;
            }

            Particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        foreach (AudioSource Audio in EnabledEffectAudioSources)
        {
            if (Audio == null)
            {
                continue;
            }

            Audio.Stop();
        }

        if (EnabledEffectRoot != null)
        {
            EnabledEffectRoot.gameObject.SetActive(false);
        }
    }

    private void CacheEnabledEffect()
    {
        if (bEffectCached)
        {
            return;
        }
        if (EnabledEffectRoot == null)
        {
            EnabledEffectRoot = FindChildRecursive(transform, EnabledEffectRootName);
        }
        EnabledEffectParticles = EnabledEffectRoot != null
            ? EnabledEffectRoot.GetComponentsInChildren<ParticleSystem>(true)
            : Array.Empty<ParticleSystem>();
        EnabledEffectAudioSources = EnabledEffectRoot != null
            ? EnabledEffectRoot.GetComponentsInChildren<AudioSource>(true)
            : Array.Empty<AudioSource>();
        bEffectCached = true;
    }

    private static Transform FindChildRecursive(Transform Root, string Name)
    {
        foreach (Transform Child in Root)
        {
            if (string.Equals(Child.name, Name, StringComparison.Ordinal))
            {
                return Child;
            }

            Transform Found = FindChildRecursive(Child, Name);
            if (Found != null)
            {
                return Found;
            }
        }

        return null;
    }
}
