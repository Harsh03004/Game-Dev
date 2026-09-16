using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour
{
    public Animator boxAnimator;
    public Animation shipAnimation;
    public GameObject enemySpaceShip;
    public GameObject canvas;
    public GameObject imageTarget;
    private EnemyShoot laserFiringScript;

    void Start()
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
    }

    public IEnumerator PlayAnimations()
    {
        if (boxAnimator == null || shipAnimation == null)
        {
            Debug.LogError("Animator or Animation component missing on BoxController!");
            yield break;
        }

        // Play box opening animation
        boxAnimator.Play("OPEN");
        yield return new WaitForSeconds(boxAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Play ship animation

        shipAnimation.Play();
        yield return new WaitForSeconds(shipAnimation.clip.length);

        // Activate enemy spaceship and UI canvas
        enemySpaceShip.SetActive(true);
        canvas.SetActive(true);

        yield return new WaitForSeconds(1.0f); // Delay after canvas activation

        if (laserFiringScript != null)
        {
            laserFiringScript.enabled = true; // Enable script
            laserFiringScript.StartFiring(); // Start firing lasers
        }
        else
        {
            Debug.LogError("Laser firing script not found on Image Target!");
        }
    }
}
