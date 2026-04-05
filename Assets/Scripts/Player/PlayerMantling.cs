using UnityEngine;
using System.Collections;

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

    private bool bCanMantling = false;
    Vector3 MantlingTargetPosition;
    void Awake()
    {
        Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        LedgeCheck();
    }
    public bool CanMantling()
    {
        if(bCanMantling)
        {
            StartCoroutine(DoMantling());
            return true;    
        }
        return false;
    }
    private IEnumerator DoMantling()
    {
        Controller.enabled = false;
        Vector3 StartPosition = transform.position;
        float duration = 1.75f;
        float time = 0;
        while(time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Vector3 NextPosition = Vector3.Lerp(StartPosition ,MantlingTargetPosition, t);
            transform.position = NextPosition;
            yield return null;
        }
        transform.position = MantlingTargetPosition;
        Controller.enabled = true;
        bCanMantling = false;
    }
    private void LedgeCheck()
    {       
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit WallHit, ForwardDistance))
        {
            Vector3 PlanCheckRayStartPosition = WallHit.point + Vector3.up * MantleHeight - WallHit.normal * 1.5f;
            if (Physics.Raycast(PlanCheckRayStartPosition, Vector3.down, out RaycastHit LedgeHit, LedgeCheckDistance))
            {
                bCanMantling = true;
                MantlingTargetPosition = LedgeHit.point + Vector3.up * 5.0f;
            }
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