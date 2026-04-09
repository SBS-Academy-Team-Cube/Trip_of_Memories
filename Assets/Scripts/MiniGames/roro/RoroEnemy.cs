using UnityEngine;

public class RoroEnemy : MonoBehaviour
{
    // If hit by the player's weapon, this object is destroyed.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerWeapon"))
        {
            gameObject.SetActive(false);
        }
    }
}
