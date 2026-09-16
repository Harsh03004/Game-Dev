using UnityEngine;
using System.Collections;

public class EnemySpaceshipEntry : MonoBehaviour
{
    public Vector3 startPosition;  // Off-screen start position
    public Vector3 entryPosition;  // Final position where the spaceship stops
    public float entrySpeed = 2f;  // Speed of movement
    public AnimationCurve entryCurve; // Smoother movement effect

    public  GameObject imageTarget;
    private MonoBehaviour laserFiringScript; // Reference to laser firing script

    void Start()
    {
        // Find the Image Target
        imageTarget = GameObject.FindWithTag("ImageTarget"); // Make sure the Image Target has this tag
        if (imageTarget != null)
        {
            // Get the laser firing script from the Image Target
            laserFiringScript = imageTarget.GetComponent<SpaceshipShooting>(); // Replace with your actual script name
            if (laserFiringScript != null)
            {
                laserFiringScript.enabled = false; // Disable laser firing initially
            }
        }
        else
        {
            Debug.LogError("Image Target not found! Make sure it's tagged as 'ImageTarget'.");
        }

        // Set spaceship off-screen
        transform.position = startPosition;

        // Start the entry movement
        StartCoroutine(SmoothEntry());
    }

    IEnumerator SmoothEntry()
    {
        float progress = 0f;
        float duration = 3f;
        Vector3 initialPosition = transform.position;

        while (progress < 1f)
        {
            progress += Time.deltaTime / duration;
            float curvedProgress = entryCurve.Evaluate(progress);
            transform.position = Vector3.Lerp(initialPosition, entryPosition, curvedProgress);
            yield return null;
        }

        transform.position = entryPosition; // Ensure final position is exact

        Debug.Log("Enemy spaceship has entered!");

        // Activate laser firing script from the Image Target
        if (imageTarget != null && laserFiringScript != null)
        {
            laserFiringScript.enabled = true;
        }
        else
        {
            Debug.LogError("Laser firing script not found on Image Target!");
        }
    }
}
