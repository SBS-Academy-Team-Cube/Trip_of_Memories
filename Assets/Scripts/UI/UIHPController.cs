using UnityEngine;

public class UIHPController : MonoBehaviour
{
    [SerializeField] private UIHPShakingEffect[] HPUIs;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip HPSFX;
    [SerializeField] private float Volume = 0.8f;
    
    private Health PlayerHP;
    public void Init(Health PlayerHP)
    {
        this.PlayerHP = PlayerHP;
        this.PlayerHP.OnHPChanged += OnHealthChanged;
    }
    void OnDisable()
    {
        if(PlayerHP != null)
        {
            PlayerHP.OnHPChanged -= OnHealthChanged;
        }
    }
    private void OnHealthChanged(int CurrentRemain)
    {
        Debug.Log($"Current HP : {CurrentRemain}");
        if (CurrentRemain > HPUIs.Length || CurrentRemain < 0)
        {
            return;
        }
        if(CurrentRemain == 3)
        {
            foreach(UIHPShakingEffect HP in HPUIs)
            {
                HP.Reset();
            }
        }
        else
        {
            HPUIs[CurrentRemain].Play();
            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(HPSFX, Volume);
            }
        }
    }
}
