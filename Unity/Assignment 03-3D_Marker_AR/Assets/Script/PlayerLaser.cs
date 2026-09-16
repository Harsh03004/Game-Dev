using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    public int damage = 10;
    public GameObject hitEffect; // Assign this in the Unity Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Ensure your enemy objects have the "Enemy" tag
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Enemy hit! Health: " + enemyHealth.CurrentHealth);
            }

            // Spawn hit effect
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f); // Destroy hit effect after 1 second
            }

            Destroy(gameObject); // Destroy the laser upon collision
        }
    }
}
