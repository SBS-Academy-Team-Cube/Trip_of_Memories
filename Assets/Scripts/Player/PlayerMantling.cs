using UnityEngine;

public class PlayerMantling : MonoBehaviour
{
    [Header("Mantle Settings")]
    public float MantleHeight = 1.5f;
    public float MantleForwardDistance = 1.0f;
    public float MantleSpeed = 5f;
    public float ForwardDistance = 1.0f;
    public float LedgeCheckDistance = 10.0f;

    [SerializeField] private Transform LedgeCheckPosition;
    private RaycastHit WallHit;
    private Vector3 WallPosition;
    private Vector3 LedgePosition;
    private CharacterController Controller;
    private bool IsMantling;
    private Vector3 TargetPosition;

    void Awake()
    {
        Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // if (IsMantling)
        // {
        //     DoMantle();
        // }
        // DebugMantleCheck();
        LedgeCheck();
    }

    private void LedgeCheck()
    {
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit WallHit, ForwardDistance))
        {
            Debug.DrawRay(transform.position, transform.forward * ForwardDistance, Color.red);
            if (Physics.Raycast(WallHit.point + Vector3.up * MantleHeight, Vector3.down, out RaycastHit LedgeHit, LedgeCheckDistance))
            {
                Debug.DrawRay(WallHit.point + Vector3.up * MantleHeight, Vector3.down * LedgeCheckDistance, Color.blue);
                Debug.Log($"Ledge Point : {LedgeHit.point}");
            }
        }
    }
    void DebugMantleCheck()
    {
        Vector3 chestOrigin = transform.position;

        Debug.DrawRay(
            chestOrigin,
            transform.forward * ForwardDistance,
            Color.red
        );
        if (Physics.Raycast(chestOrigin, transform.forward, out RaycastHit wallHit, ForwardDistance))
        {
            Debug.DrawLine(chestOrigin, wallHit.point, Color.green);

            Vector3 topOrigin = wallHit.point + Vector3.up * MantleHeight;

            Debug.DrawRay(
                topOrigin,
                Vector3.down * 10.0f,
                Color.blue
            );

            if (Physics.Raycast(topOrigin, Vector3.down, out RaycastHit downHit, 10f))
            {
                Debug.DrawLine(topOrigin, downHit.point, Color.yellow);
            }
        }
        else
        {
            Debug.Log("Can't Raycast");
        }
    }
    public bool CanMantle()
    {
        if (IsMantling) return false;

        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.0f;

        if (Physics.Raycast(origin, transform.forward, out hit, MantleForwardDistance))
        {
            Vector3 topCheck = hit.point + Vector3.up * MantleHeight;

            if (!Physics.Raycast(topCheck, Vector3.down, 1.0f))
            {
                TargetPosition = topCheck;
                return true;
            }
        }

        return false;
    }
    public void StartMantle()
    {
        IsMantling = true;
        Controller.enabled = false;
    }
    private void DoMantle()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            TargetPosition,
            MantleSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, TargetPosition) < 0.1f)
        {
            IsMantling = false;
            Controller.enabled = true;
        }
    }
    public bool IsMantlingNow()
    {
        return IsMantling;
    }
}