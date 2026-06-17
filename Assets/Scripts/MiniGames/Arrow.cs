using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10f;
    private Vector3 MoveDirection;

    public void Init(Vector3 Direction)
    {
        MoveDirection = Direction.normalized;

        if (MoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-MoveDirection);
        }
    }

    void Update()
    {
        transform.position += MoveDirection * MoveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider Other)
    {
        if (Other.TryGetComponent(out Health HP) && Other.CompareTag("Player"))
        {
            HP.TakeDamage();
        }
        Destroy(gameObject);
    }
}
