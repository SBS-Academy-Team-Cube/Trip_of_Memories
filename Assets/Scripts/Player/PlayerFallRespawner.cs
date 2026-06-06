using System.Collections;
using UnityEngine;

public class PlayerFallRespawner : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;
    public struct PlayerTransformSnapshot
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public PlayerTransformSnapshot(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
    private CircularQueue<PlayerTransformSnapshot> safeTransforms;
    [SerializeField] private int SafeFrameCount = 30;
    private bool bWasGrounded;
    private void Update()
    {
        if (Controller == null)
        {
            return;
        }

        bool bIsGrounded = Controller.isGrounded;
        if (bIsGrounded)
        {
            if (!bWasGrounded)
            {
                safeTransforms.Clear();
            }

            SaveTrans();
        }
        bWasGrounded = bIsGrounded;
    }
    private void Awake()
    {
        if (Controller == null)
        {
            TryGetComponent(out Controller);
        }
        safeTransforms = new CircularQueue<PlayerTransformSnapshot>(Mathf.Max(1, SafeFrameCount));
        bWasGrounded = Controller != null && Controller.isGrounded;
    }
    public void Fall()
    {
        if (Controller == null || safeTransforms.Count == 0)
        {
            return;
        }

        Controller.enabled = false;

        var SafeTransform = safeTransforms.Peek();
        transform.SetPositionAndRotation(SafeTransform.Position, SafeTransform.Rotation);

        Controller.enabled = true;

        StartCoroutine(RespawnEffect());
    }
    private void SaveTrans()
    {
        safeTransforms.Add(new PlayerTransformSnapshot(transform.position, transform.rotation));
    }
    private IEnumerator RespawnEffect()
    {
        // effect
        yield return null;
    }
}
