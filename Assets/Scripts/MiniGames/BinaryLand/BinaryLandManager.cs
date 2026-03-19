using UnityEngine;

public class BinaryLandManager : MonoBehaviour
{
    [Header("PlayerSetting")]
    [SerializeField] private BinaryLandPlayerMovement Player1;
    [SerializeField] private BinaryLandPlayerMovement Player2;

    [Header("ClearCheckComponent")]
    [SerializeField] private BinaryLandClearCheck Player1Check;
    [SerializeField] private BinaryLandClearCheck Player2Check;

    private bool IsCleared = false;

    private void Awake()
    {
        if (Player1 == null || Player2 == null)
            Debug.Log("BinaryLandManager.cs - Awake() - Player Set error");
        if (Player1Check == null || Player2Check == null)
            Debug.Log("BinaryLandManager.cs - Awake() - ClearCheck Set error");

        IsCleared = false;
    }

    private void Update()
    {
        if (IsCleared)
            return;

        if(!Player1.IsMoving && !Player2.IsMoving && 
            Player1Check.GoalCheck && Player2Check.GoalCheck)
        {
            BinaryLandClear();
        }
    }

    private void BinaryLandClear()
    {
        IsCleared = true;
        Debug.Log("BinaryLand Clear");
    }


}
