using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private GameObject ArrowPrefab = null;
    [SerializeField] private Transform MuzzlePosition = null;
    [SerializeField] private AudioSource AudioSource = null;
    [SerializeField] private AudioClip SFX = null;
    [SerializeField] private float FireRate = 2.5f;

    private float Timer = 0.0f;
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= FireRate)
        {
            var Instance = Instantiate(ArrowPrefab, MuzzlePosition.position, MuzzlePosition.rotation);
            if (!Instance.TryGetComponent(out Arrow arrow))
            {
                Destroy(Instance);
                return;
            }
            if (AudioSource && SFX)
            {
                AudioSource.PlayOneShot(SFX, AudioSource.volume * (AudioManager.Instance ? AudioManager.Instance.SFX_VOLUME : 1.0f));
            }

        }
    }

}