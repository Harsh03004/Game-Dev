using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    public int damage = 10; // Damage per hit
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("MainCamera")) // If asteroid hits the camera
        {
            Debug.Log("Asteroid hit the player");
            PlayerHealth.Instance.TakeDamage(damage); // Reduce player health
            Destroy(gameObject); // Destroy asteroid
        }
    }


}
