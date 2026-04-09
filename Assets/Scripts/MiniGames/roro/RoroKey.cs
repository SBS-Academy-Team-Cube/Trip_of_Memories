using System.Collections;
using UnityEngine;

public class RoroKey : MonoBehaviour
{
    // It would be nice to add a coroutine motion that moves up and down...
    // If the player triggers this, publish an event that makes the key disappear and opens the door.
    [SerializeField] private GameObject Door;
    [SerializeField] private float RotateSpeed = 20f;
    private float MovePosY = 0f;
    private float StartPosY;
    private bool IsUp = false;
    private Coroutine KeyMovingCoroutine;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Door != null && Door.TryGetComponent<RoroDoor>(out RoroDoor component))
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                component.OpenDoor();
            }
        }
    }
    private void Awake()
    {
        StartPosY = transform.position.y;
    }

    private void OnEnable()
    {
        KeyMovingCoroutine = StartCoroutine(KeyMoving());
    }
    private void OnDisable()
    {
        if(KeyMovingCoroutine != null)
            StopCoroutine(KeyMovingCoroutine);
    }
    private IEnumerator KeyMoving()
    {
        yield return null;
        while(true)
        {
            if(MovePosY <= 0f)
                IsUp = true;
            else if(MovePosY >= 1f)
                IsUp= false;

            if (IsUp)
            {
                MovePosY += Time.deltaTime;
                transform.position = new Vector3(transform.position.x,
                    StartPosY + MovePosY, transform.position.z);
                transform.Rotate(Vector3.up * (RotateSpeed * Time.deltaTime));
            }
            else
            {
                MovePosY -= Time.deltaTime;
                transform.position = new Vector3(transform.position.x,
                    StartPosY + MovePosY, transform.position.z);
                transform.Rotate(Vector3.up * (RotateSpeed * Time.deltaTime));
            }
            yield return null;
        }
    }
}
