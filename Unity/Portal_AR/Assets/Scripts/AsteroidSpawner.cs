using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; // Array of different asteroid types
    public Transform playerCamera; // Assign the AR Camera (Main Camera)
    public Transform portalPlane; // Reference to the dynamically spawned PortalPlane (now the exit trigger)

    public float spawnDistance = 10f; // Increased spawn distance for far-away effect
    public float initialAsteroidSpeed = 0.5f; // Start slow
    public float maxAsteroidSpeed = 8f; // Cap speed at a reasonable max
    private float currentAsteroidSpeed; // Dynamic speed

    public int initialMinAsteroids = 2, initialMaxAsteroids = 3; // Start with fewer asteroids
    public int maxAsteroidsCap = 8; // Maximum number of asteroids
    private int currentMinAsteroids, currentMaxAsteroids; // Dynamic asteroid count

    public float minScale = 0.1f, maxScale = 1f; // Asteroid starts small and grows

    public float difficultyIncreaseInterval = 10f; // Every 10 seconds, increase difficulty
    public float difficultyStep = 0.5f; // Amount to increase speed each step

    void Start()
    {
        currentAsteroidSpeed = initialAsteroidSpeed;
        currentMinAsteroids = initialMinAsteroids;
        currentMaxAsteroids = initialMaxAsteroids;

        // Ensure portalPlane has a collider and set to trigger in Unity editor
        if (portalPlane != null && portalPlane.GetComponent<Collider>() == null)
        {
            portalPlane.gameObject.AddComponent<BoxCollider>().isTrigger = true; // Add BoxCollider if none exists
        }

        StartCoroutine(FindPortalAndSpawnAsteroids());
        StartCoroutine(IncreaseDifficulty());
    }

    IEnumerator FindPortalAndSpawnAsteroids()
    {
        // Wait until Portal spawns
        while (portalPlane == null)
        {
            GameObject portal = GameObject.FindWithTag("Portal"); // Find Portal
            if (portal != null)
            {
                Transform portalBlue = portal.transform.Find("Portal blue"); // Find PortalBlue inside Portal
                if (portalBlue != null)
                {
                    portalPlane = portalBlue.Find("portalPlane"); // Find PortalPlane inside PortalBlue
                }
            }
            yield return null; // Wait until the next frame if not found
        }

        StartCoroutine(SpawnAsteroids());
    }

    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            int asteroidCount = Random.Range(currentMinAsteroids, currentMaxAsteroids + 1); // Gradual increase
            for (int i = 0; i < asteroidCount; i++)
            {
                SpawnAsteroid();
            }

            yield return new WaitForSeconds(2f); // Spawn every 2 seconds
        }
    }

    void SpawnAsteroid()
    {

        if (playerCamera == null || portalPlane == null || asteroidPrefabs.Length == 0) return;

        // Choose a random asteroid type
        GameObject asteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Get a position opposite the portalPlane
        Vector3 spawnPosition = GetSpawnPositionOppositePortal();

        // Instantiate asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Set initial small scale
        asteroid.transform.localScale = Vector3.one * minScale;
        // Add Rigidbody if not already present
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        if (rb == null)
            rb = asteroid.AddComponent<Rigidbody>();

        rb.useGravity = false; // Disable gravity so asteroids don't fall
        rb.isKinematic = false; // Allow physics interaction
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Better collision detection

        // Ensure asteroid has a collider
        if (asteroid.GetComponent<Collider>() == null)
            asteroid.AddComponent<SphereCollider>(); // Change to BoxCollider if needed

        // Ensure asteroid moves towards the player
        Vector3 directionToPlayer = (playerCamera.position - spawnPosition).normalized;
        rb.linearVelocity = directionToPlayer * currentAsteroidSpeed;

        // Make the asteroid grow as it gets closer
        StartCoroutine(GrowAsteroid(asteroid));

        // Destroy asteroid after 10 seconds if it doesn't hit anything
        Destroy(asteroid, 10f);
    }

    Vector3 GetSpawnPositionOppositePortal()
    {
        if (portalPlane == null || playerCamera == null)
            return Vector3.zero;

        // Get the player's forward direction
        Vector3 playerForward = playerCamera.forward;

        // Get the portal's forward direction
        Vector3 portalForward = portalPlane.forward;

        // Compute the opposite direction from the portal (but facing the player)
        Vector3 spawnDirection = -portalForward; // Opposite of portal
        spawnDirection.y = 0; // Ensure it's roughly on the same height level

        // Get a base spawn position at the desired distance
        Vector3 spawnPosition = playerCamera.position + spawnDirection * spawnDistance;

        // Apply some slight randomness to avoid uniform spawning
        spawnPosition += new Vector3(Random.Range(-10f, 10f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        return spawnPosition;
    }


    IEnumerator GrowAsteroid(GameObject asteroid)
    {
        float growDuration = 3f; // Time to reach full size
        float elapsedTime = 0f;
        Vector3 targetScale = Vector3.one * maxScale;

        while (elapsedTime < growDuration)
        {
            asteroid.transform.localScale = Vector3.Lerp(Vector3.one * minScale, targetScale, elapsedTime / growDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        asteroid.transform.localScale = targetScale; // Ensure it reaches full size
    }

    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);

            // Increase speed gradually but cap it
            if (currentAsteroidSpeed < maxAsteroidSpeed)
            {
                currentAsteroidSpeed += difficultyStep;
            }

            // Increase asteroid count gradually but cap it
            if (currentMaxAsteroids < maxAsteroidsCap)
            {
                currentMinAsteroids++;
                currentMaxAsteroids++;
            }

            Debug.Log($"Difficulty Increased! Speed: {currentAsteroidSpeed}, Asteroids: {currentMinAsteroids}-{currentMaxAsteroids}");
        }
    }
}
