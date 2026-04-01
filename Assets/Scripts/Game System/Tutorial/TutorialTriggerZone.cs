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
    private void OnTriggerEnter(Collider Other)
    {
        if (Triggered)
        {
            return;
        }
        if (!Other.CompareTag("Player"))
        {
            return;
        }
        Triggered = true;
        if (Manager != null)
        {
            Manager.ShowTutorialText(Index);
        }
    }
}
