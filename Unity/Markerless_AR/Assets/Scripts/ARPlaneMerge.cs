using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneMerge : MonoBehaviour
{
    public ARPlaneManager planeManager; // Reference to ARPlaneManager
    private ARPlane mainPlane; // The main merged plane

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
        MergeAllPlanes();
    }

    private void MergeAllPlanes()
    {
        if (planeManager.trackables.count == 0) return;

        FindMainPlane(); // Identify the lowest plane

        if (mainPlane == null) return;

        List<ARPlane> planesToRemove = new List<ARPlane>();

        foreach (var plane in planeManager.trackables)
        {
            if (plane != mainPlane && IsOverlapping(plane, mainPlane))
            {
                ExpandMainPlane(plane);
                planesToRemove.Add(plane);
            }
        }

        // Remove all merged planes
        foreach (var plane in planesToRemove)
        {
            Destroy(plane.gameObject);
        }
    }

    // Find the lowest plane (smallest Y position)
    private void FindMainPlane()
    {
        float minY = float.MaxValue;
        mainPlane = null;

        foreach (var plane in planeManager.trackables)
        {
            if (plane.transform.position.y < minY)
            {
                minY = plane.transform.position.y;
                mainPlane = plane;
            }
        }
    }

    // Check if two planes overlap
    private bool IsOverlapping(ARPlane smallPlane, ARPlane largePlane)
    {
        float distance = Vector3.Distance(smallPlane.center, largePlane.center);
        return distance < (largePlane.extents.magnitude * 1.5f); // Expand merge range slightly
    }

    // Expand the main plane to include the merged plane
    private void ExpandMainPlane(ARPlane mergingPlane)
    {
        Vector2 newSize = mainPlane.size + mergingPlane.size;
        mainPlane.gameObject.transform.localScale = new Vector3(newSize.x, 1, newSize.y);
    }
}
