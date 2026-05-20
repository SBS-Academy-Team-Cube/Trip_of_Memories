using UnityEngine;

public interface IAudible3D
{
    bool IsAudible { get; }
    bool IsWithinAudibleRange { get; }

    void RefreshAudibility();
    void PlayOneShot(AudioClip Clip);
    void Stop();
}
