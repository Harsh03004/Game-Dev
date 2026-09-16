using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleAnimationTest : MonoBehaviour
{
    public GameObject iPhoneModel;  // The whole iPhone model
    public Button batteryButton;    // Button to focus on the battery
    public Button backButton;       // Button to go back to teardown view
    public Button teardownButton;   // Button to trigger teardown animation
    public Transform batteryPart;   // Battery part inside the body

    private Vector3 batteryTeardownPos;   // Store the battery's position after teardown animation
    private Quaternion batteryTeardownRot; // Store the battery's rotation after teardown animation

    private Animation animationComponent;
    private bool isBatteryRotated = false;
    private bool teardownAnimationPlayed = false;

    void Start()
    {
        // Initialize the iPhone model and animation component
        if (iPhoneModel != null)
        {
            iPhoneModel.SetActive(true);
            animationComponent = iPhoneModel.GetComponent<Animation>();

            if (animationComponent != null)
            {
                Debug.Log("Animation component found on iPhone model.");
            }
            else
            {
                Debug.LogError("Animation component missing on iPhone model.");
            }
        }
        else
        {
            Debug.LogError("iPhone model not assigned.");
        }

        // Set up button listeners
        if (batteryButton != null)
            batteryButton.onClick.AddListener(OnBatteryButtonClick);

        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClick);

        if (teardownButton != null)
            teardownButton.onClick.AddListener(OnTeardownButtonClick);
    }

    // Function to handle the teardown button click
    void OnTeardownButtonClick()
    {
        if (animationComponent != null)
        {
            animationComponent["Take 001-D"].speed = 2.0f;  // Play at 2x speed
            animationComponent.Play("Take 001-D");
            Debug.Log("Playing teardown animation 'Take 001-D' at double speed.");

            // Start coroutine to wait for animation to finish and save battery position
            StartCoroutine(SaveBatteryPositionAfterAnimation());
        }
        else
        {
            Debug.LogError("Animation component not found to play 'Take 001-D'.");
        }
    }

    // Coroutine to check when the teardown animation is finished and save battery position
    private IEnumerator SaveBatteryPositionAfterAnimation()
    {
        while (animationComponent.isPlaying) // Wait until animation stops
        {
            yield return null;
        }

        // Store the battery's final position and rotation after the teardown animation
        if (batteryPart != null)
        {
            batteryTeardownPos = batteryPart.position;
            batteryTeardownRot = batteryPart.rotation;
            teardownAnimationPlayed = true; // Mark that the teardown animation has completed
            Debug.Log("Teardown animation finished. Battery position saved.");
        }
    }

    // Function to handle the battery button click
    void OnBatteryButtonClick()
    {
        if (teardownAnimationPlayed && !isBatteryRotated && batteryPart != null)
        {
            isBatteryRotated = true;
            RotateBattery();  // Rotate by 90 degrees on the Z-axis
        }
    }


    // Function to handle the back button click
    void OnBackButtonClick()
    {
        if (isBatteryRotated && batteryPart != null)
        {
            isBatteryRotated = false;
            ResetBatteryToTeardownPosition();  // Reset the battery to the saved teardown position
        }
    }

    // Function to rotate the battery by a specified angle on the X-axis
    void RotateBattery()
    {
        if (batteryPart != null)
        {
            batteryPart.rotation = Quaternion.Euler(
                batteryPart.rotation.eulerAngles.x + 90,
                batteryPart.rotation.eulerAngles.y,
                batteryPart.rotation.eulerAngles.z + 90
            );

            batteryPart.localScale *= 2.5f; // 1.5x bigger
        }
    }

    // Function to reset the battery's position and rotation to its teardown state
    void ResetBatteryToTeardownPosition()
    {
        if (teardownAnimationPlayed && batteryPart != null)
        {
            batteryPart.position = batteryTeardownPos;
            batteryPart.rotation = batteryTeardownRot;
            batteryPart.localScale = Vector3.one; // Reset scale to original
            Debug.Log("Battery reset to teardown position.");
        }
    }

}