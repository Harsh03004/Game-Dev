using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int CurrentHealth { get; private set; }
    public HealthBar healthBar;
    private EnemyShoot laserFiringScript;
    public GameObject imageTarget;
    public GameObject explosionEffect; // Assign this in the Unity Inspector

    void Start()
    {
        CurrentHealth = maxHealth;
        Debug.Log("Enemy Health Initialized: " + CurrentHealth);
        healthBar.SetMaxHealth(CurrentHealth);
    }

    // Call this function when the enemy is hit
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log("Enemy hit! Health: " + CurrentHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(CurrentHealth);  // Update the health bar with the current health
        }

        if (CurrentHealth <= 0)
        {
            imageTarget = GameObject.FindWithTag("ImageTarget"); // Ensure Image Target has this tag

            if (imageTarget != null)
            {
                laserFiringScript = imageTarget.GetComponent<EnemyShoot>();
                if (laserFiringScript != null)
                {
                    laserFiringScript.enabled = false; // Disable laser firing initially
                }
            }
            else
            {
                Debug.LogError("Image Target not found! Make sure it's tagged as 'ImageTarget'.");
            }

            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy destroyed!");

        // Spawn explosion effect at enemy's position
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Destroy explosion effect after 2 seconds
        }
        else
        {
            Debug.LogError("Explosion effect not assigned!");
        }

        // Disable the enemy
        gameObject.SetActive(false);
    }
}
