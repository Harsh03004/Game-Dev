using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public RawImage damageAffect;
    public float damageEffectDuration = 0.5f;
    public Text health;
    public RawImage gameOverScreen;
    public Text gameOver;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        damageAffect.enabled = false;
        gameOverScreen.enabled = false;
        gameOver.enabled = false;
    }

    public void TakeDamage(int damage)  // Changed to public so other scripts can call it
    {
        if (isDead) return;

        currentHealth-=damage;
        Debug.Log(currentHealth);
        UpdateHealthUI();
        StartCoroutine(ShowDamageAffect());

        if (currentHealth <= 0)
        {
            DIE();
        }
    }

    void UpdateHealthUI()
    {
        health.text = "Health: " + currentHealth.ToString();  // Added a space after "Health"
    }

    IEnumerator ShowDamageAffect()
    {
        damageAffect.enabled = true;
        yield return new WaitForSeconds(damageEffectDuration);
        damageAffect.enabled = false;
    }

    void DIE()
    {
        gameOverScreen.enabled = true;
        gameOver.enabled = true;
        isDead = true;
        Debug.Log("Player has died!");  // Added a debug log for better tracking
    }
}
