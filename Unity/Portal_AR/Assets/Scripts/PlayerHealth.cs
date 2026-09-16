using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    public int maxHealth = 100;
    public RawImage damageAffect;
    public float damageEffectDuration = 0.4f;
    public RawImage gameOverScreen;
    private int currentHealth;
    public Text healthText; // Assign a UI Text for health display

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        damageAffect.enabled = false;
        gameOverScreen.enabled = false;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Asteroid hit the player" + currentHealth);
        UpdateHealthUI();
        StartCoroutine(ShowDamageAffect());

        // Trigger camera shake when hit
        if (CameraShake.Instance != null)
        {
            StartCoroutine(CameraShake.Instance.Shake(0.3f, 0.2f)); // Adjust duration & intensity
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Game Over! Player died.");
            gameOverScreen.enabled = true;
        }
    }

    IEnumerator ShowDamageAffect()
    {
        damageAffect.enabled = true;
        yield return new WaitForSeconds(damageEffectDuration);
        damageAffect.enabled = false;
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
    }
}
