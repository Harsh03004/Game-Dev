using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid")) // Ensure asteroids have this tag
        {
            Debug.Log("Asteroid entered the portal and was destroyed!"); // Debug message
            Destroy(other.gameObject);
        }
    }
}
