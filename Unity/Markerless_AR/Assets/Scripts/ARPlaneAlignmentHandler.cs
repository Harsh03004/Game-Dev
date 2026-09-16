using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneAlignmentHandler : MonoBehaviour
{
    public ARPlaneManager planeManager;

    private void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (ARPlane plane in args.added)
        {
            AdjustPlanePosition(plane);
        }

        foreach (ARPlane plane in args.updated)
        {
            AdjustPlanePosition(plane);
        }
    }

    private void AdjustPlanePosition(ARPlane plane)
    {
        // Align the plane based on its detected orientation
        if (plane.alignment == PlaneAlignment.HorizontalUp)
        {
            // For floors: Make sure the plane is flat and aligned with the ground
            plane.transform.rotation = Quaternion.Euler(0, plane.transform.rotation.eulerAngles.y, 0);
        }
        else if (plane.alignment == PlaneAlignment.Vertical)
        {
            // For walls: Ensure it's upright with correct rotation
            plane.transform.rotation = Quaternion.Euler(0, plane.transform.rotation.eulerAngles.y, 90);
        }
    }
}
