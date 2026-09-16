using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float attackRange = 3f;  // Range within which the enemy can attack
    public int damagePerHit = 1;    // Damage inflicted per attack
    public float attackCooldown = 1.5f;  // Delay between attacks

    public GameObject player;
    public PlayerHealth playerHealth;
    private bool canAttack = true;  // Ensures the enemy waits between attacks

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null || playerHealth == null || playerHealth.enabled == false) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        // Debug.Log("Distance to Player: " + distance);

        if (distance <= attackRange && canAttack)
        {
            StartCoroutine(AttackPlayer());
        }
    }


    IEnumerator AttackPlayer()
    {
        canAttack = false;
        Debug.Log("Enemy attacking!"); // Check if attack is actually happening

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
            Debug.Log("Enemy dealt damage!");
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
