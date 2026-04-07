using UnityEngine;

public class TutorialTriggerZone : MonoBehaviour
{
    private int Index;
    private TutorialManager Manager;
    private bool Triggered = false;

    public void Init(TutorialManager Manager, int Index)
    {
        this.Manager = Manager;
        this.Index = Index;
    }

    public void ResetTrigger()
    {
        Triggered = false;
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (Triggered)
            return;

        if (!Other.CompareTag("Player"))
            return;

        Triggered = true;

        Manager?.ShowTutorialText(Index);
    }
}