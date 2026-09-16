using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;
    public BoxController boxController; // Reference to BoxController
    public GameObject countdownCanvas; // Reference to the Canvas displaying the countdown

    private void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSeconds(2f);
        Debug.Log(countdownDisplay.text);

        // Deactivate countdown canvas
        if (countdownCanvas != null)
        {
            countdownCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("Countdown Canvas not assigned in CountdownController!");
        }

        yield return new WaitForSeconds(0.5f); // Small delay to ensure smooth transition

        // Start the box opening animation
        if (boxController != null)
        {
            StartCoroutine(boxController.PlayAnimations());
        }
        else
        {
            Debug.LogError("BoxController reference is missing in CountdownController!");
        }
    }
}
