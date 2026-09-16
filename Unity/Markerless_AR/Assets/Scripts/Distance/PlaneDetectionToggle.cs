using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionButton : MonoBehaviour
{
    public ARPlaneManager arPlaneManager;
    public Button planeToggleButton;
    private bool isPlaneDetectionActive = false;

    void Start()
    {
        arPlaneManager.enabled = false; // Disable plane detection at the start
        SetPlaneVisibility(false);

        if (planeToggleButton != null)
        {
            planeToggleButton.onClick.AddListener(TogglePlaneDetection);
        }
    }

    public void TogglePlaneDetection()
    {
        isPlaneDetectionActive = !isPlaneDetectionActive;
        arPlaneManager.enabled = isPlaneDetectionActive;
        SetPlaneVisibility(isPlaneDetectionActive);

        // Change button text (optional)
        planeToggleButton.GetComponentInChildren<Text>().text = isPlaneDetectionActive ? "Stop Plane Detection" : "Start Plane Detection";
    }

    void SetPlaneVisibility(bool isVisible)
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(isVisible);
        }
    }
}
