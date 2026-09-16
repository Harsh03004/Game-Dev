using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TogglePlaneDetection : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public Button toggleButton;
    private bool arePlanesVisible = true; // Track visibility of planes

    void Start()
    {
        UpdateButtonText();
        toggleButton.onClick.AddListener(TogglePlanes);
    }

    void TogglePlanes()
    {
        arePlanesVisible = !arePlanesVisible;

        // Toggle visibility of all detected planes
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(arePlanesVisible);
        }

        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        if (toggleButton != null)
        {
            var buttonText = toggleButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = arePlanesVisible ? "Hide Planes" : "Show Planes";
            }
        }
    }
}
