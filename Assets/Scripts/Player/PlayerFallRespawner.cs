using System.Collections;
using UnityEngine;

public class PlayerFallRespawner : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;
    private Vector3 lastSafeTrans;
    private Quaternion lastRotate;

    private void Update()
    {
        if(Controller != null && Controller.isGrounded)
        {
            SaveTrans();
        }
    }
    private void Awake()
    {
        if(Controller == null)
        {
            TryGetComponent(out Controller);
        }
    }
    public void Fall()
    {
        Controller.enabled = false;

        transform.position = lastSafeTrans; 
        transform.rotation = lastRotate;

        Controller.enabled = true;
        
        StartCoroutine(RespawnEffect());
    }
    private void SaveTrans()
    {
        lastSafeTrans = transform.position;
        lastRotate = transform.rotation;
    }
    private IEnumerator RespawnEffect()
    {
        // effect
        yield return null;
    }
}
