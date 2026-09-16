using UnityEngine;
using System.Collections;

public class SpaceshipShooting : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public float laserSpeed = 5f;
    public float fireRate = 0.5f;
    public float moveSpeed = 5f;
    public float gyroSensitivity = 2f;
    public int health = 100;  // Health system

    private float nextFireTime = 0f;
    private bool isGyroAvailable = false;

    void Start()
    {
        // Enable gyroscope if available
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            isGyroAvailable = true;
        }
    }

    void Update()
    {
        if (isGyroAvailable)
        {
            HandleGyroMovement();
        }
        else
        {
            HandleKeyboardMovement();
        }

        // Fire laser on left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            FireLaser();
        }

        // Detect touch input for shooting
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            FireLaser();
        }
    }

    public void FireLaser()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        if (laserPrefab == null || firePoint1 == null || firePoint2 == null) return;

        // Instantiate lasers at both fire points
        GameObject laser1 = Instantiate(laserPrefab, firePoint1.position, firePoint1.rotation);
        GameObject laser2 = Instantiate(laserPrefab, firePoint2.position, firePoint2.rotation);

        // Start coroutines for movement
        StartCoroutine(MoveLaser(laser1, firePoint1.forward));
        StartCoroutine(MoveLaser(laser2, firePoint2.forward));
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
    }

    void HandleKeyboardMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void HandleGyroMovement()
    {
        Vector3 gyroRotation = Input.gyro.rotationRateUnbiased;
        float moveX = gyroRotation.y * gyroSensitivity;
        float moveZ = -gyroRotation.x * gyroSensitivity;

        Vector3 movement = new Vector3(moveX, 0, moveZ) * Time.deltaTime;
        transform.Translate(movement);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            health = 0;  // Set health to zero
            Destroy(gameObject);  // Destroy spaceship
            Debug.Log("Spaceship destroyed!");
        }
    }
}

