using UnityEngine;
using Vuforia;
using System.Collections;

public class EnemyShoot : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject hitEffect; // Assign this in Unity Inspector
    public Transform firePoint;
    public float laserSpeed = 5f;
    public float fireRate = 1.0f;

    private bool isMarkerDetected = false;
    private Coroutine fireRoutine;
    private ObserverBehaviour observerBehaviour;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
            Debug.Log("ObserverBehaviour detected and event attached.");
        }
        else
        {
            Debug.LogError("ObserverBehaviour component is missing! Make sure this script is attached to an ImageTarget.");
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        Debug.Log("Marker status changed: " + status.Status);

        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            if (!isMarkerDetected)
            {
                isMarkerDetected = true;
                Debug.Log("Marker detected! Starting firing...");
                StartFiring();
            }
        }
        else
        {
            if (isMarkerDetected)
            {
                isMarkerDetected = false;
                Debug.Log("Marker lost! Stopping firing...");
                StopFiring();
            }
        }
    }

    public void StartFiring()
    {
        if (!isMarkerDetected)
        {
            isMarkerDetected = true;
        }

        if (fireRoutine == null)
        {
            fireRoutine = StartCoroutine(FireRoutine());
        }
    }

    void StopFiring()
    {
        if (fireRoutine != null)
        {
            StopCoroutine(fireRoutine);
            fireRoutine = null;
        }
    }

    IEnumerator FireRoutine()
    {
        yield return new WaitForSeconds(1f); // Initial delay before firing
        while (isMarkerDetected)
        {
            FireLaser();
            yield return new WaitForSeconds(fireRate);
        }
    }

    public void FireLaser()
    {
        if (!isMarkerDetected)
        {
            Debug.LogWarning("Tried to fire, but marker is not detected!");
            return;
        }

        if (laserPrefab == null)
        {
            Debug.LogError("Laser prefab is not assigned in Inspector!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned in Inspector!");
            return;
        }

        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        laser.tag = "EnemyLaser";
        Debug.Log("Laser fired from position: " + firePoint.position);
        StartCoroutine(MoveLaser(laser, firePoint.forward));
    }

    IEnumerator MoveLaser(GameObject laser, Vector3 direction)
    {
        float lifetime = 5f;
        float elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            laser.transform.position += direction * laserSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(laser);
        Debug.Log("Laser destroyed after lifetime.");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Laser collided with: " + other.gameObject.name); // Debug

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit!"); // Debug
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                Debug.Log("Player health: " + playerHealth.currentHealth);
            }

            // Spawn hit effect
            if (hitEffect != null)
            {
                Debug.Log("Spawning hit effect...");
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
            }
            else
            {
                Debug.LogError("Hit effect is NULL! Assign a particle effect prefab in the Inspector.");
            }

            Destroy(gameObject);
        }
    }

}
