using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animation anim;

    public HealthBar healthBar;  // Reference to the HealthBar script
    public GameObject imageTarget;
    private EnemyShoot laserFiringScript;

    public GameObject explosionEffect; // Assign this in Unity Inspector
    public GameObject hitEffect;  // **New: Assign a hit effect in Inspector**

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);  // Initialize the health bar
        Debug.Log("Player Health Initialized: " + currentHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyLaser")) // Ensure the laser has this tag
        {
            TakeDamage(10); // Reduce health by 10 per hit
            Destroy(other.gameObject); // Destroy the laser on impact
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player hit! Health: " + currentHealth);

        healthBar.SetHealth(currentHealth); // Update the health bar when the player takes damage

        // **NEW: Spawn hit effect**
        if (hitEffect != null)
        {
            GameObject hitFX = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(hitFX, 1f); // Destroy effect after 1 second
        }
        else
        {
            Debug.LogError("Hit effect not assigned!");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        imageTarget = GameObject.FindWithTag("ImageTarget"); // Ensure Image Target has this tag

        if (imageTarget != null)
        {
            laserFiringScript = imageTarget.GetComponent<EnemyShoot>();
            if (laserFiringScript != null)
            {
                laserFiringScript.enabled = false; // Disable laser firing
            }
        }
        else
        {
            Debug.LogError("Image Target not found! Make sure it's tagged as 'ImageTarget'.");
        }

        Debug.Log("Player has been destroyed!");

        // Spawn explosion effect at player's position
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Destroy explosion effect after 2 seconds
        }
        else
        {
            Debug.LogError("Explosion effect not assigned!");
        }

        if (anim != null)
        {
            Debug.Log("Playing death animation.");
            anim.Play();
        }
        else
        {
            Debug.LogError("Animation component not assigned!");
        }

        // Disable the player
        gameObject.SetActive(false);
    }
}
