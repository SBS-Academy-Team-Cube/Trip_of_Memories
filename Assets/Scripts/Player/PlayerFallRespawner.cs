using System.Collections;
using UnityEngine;

public class PlayerFallRespawner : MonoBehaviour
{
    private CharacterController controller;

    private Vector3 lastSafeTrans;
    private Quaternion lastRotate;

    private void Update()
    {
        if(controller != null && controller.isGrounded)
        {
            SaveTrans();
        }
    }


    public void Init()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Fall()
    {
        StartCoroutine(ControllerLock());

        transform.position = lastSafeTrans;
        transform.rotation = lastRotate;

        StartCoroutine(RespawnEffect());
    }
    private void SaveTrans()
    {
        lastSafeTrans = transform.position;
        lastRotate = transform.rotation;
    }

    private IEnumerator ControllerLock()
    {
        controller.enabled = false;
        yield return new WaitForSeconds(0.1f);

        controller.enabled = true;
    }

    private IEnumerator RespawnEffect()
    {
        // effect
        yield return null;
    }
}
