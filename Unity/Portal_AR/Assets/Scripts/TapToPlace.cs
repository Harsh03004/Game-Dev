using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PortalSpawner : MonoBehaviour
{
    public GameObject prefabPortal; // The portal prefab to spawn
    private GameObject spawnedPortal; // Reference to the spawned portal
    private ARAnchor currentAnchor; // Stores the current anchor

    public ARPlaneManager planeManager; // AR Plane Manager for detecting planes
    public ARAnchorManager anchorManager; // AR Anchor Manager for anchoring objects
    public ARRaycastManager raycastManager; // AR Raycast Manager for detecting tap positions

    private bool portalSpawned = false; // Flag to track if the portal is already placed
    private bool planesDetected = false; // Track if planes are detected

    void Start()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (portalSpawned || planesDetected) return;

        if (args.added.Count > 0)
        {
            planesDetected = true; // ✅ Planes detected, now allow tapping
            Debug.Log("✅ Planes detected! Tap the screen or click with left mouse to place the portal.");
        }
    }

    void Update()
    {
        if (portalSpawned || !planesDetected) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandlePlacement(Input.GetTouch(0).position);
        }

        // ✅ Left mouse button click (for PC testing)
        if (Input.GetMouseButtonDown(0))
        {
            HandlePlacement(Input.mousePosition);
        }
    }

    void HandlePlacement(Vector2 screenPosition)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            ARPlane hitPlane = planeManager.GetPlane(hits[0].trackableId);

            if (hitPlane != null)
            {
                PlacePortal(hitPose, hitPlane);
            }
        }
        else
        {
            // **Fallback: Use Physics.Raycast if AR Raycast fails**
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                PlacePortal(new Pose(hit.point, Quaternion.identity), null);
            }
        }
    }

    void PlacePortal(Pose spawnPose, ARPlane plane)
    {
        if (portalSpawned) return;

        // Ensure the portal faces forward correctly
        Quaternion portalRotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));

        // Spawn the portal at the tapped position
        spawnedPortal = Instantiate(prefabPortal, spawnPose.position, portalRotation);

        if (anchorManager != null)
        {
            // ✅ Instead of attaching to the plane, create a new standalone world-space anchor
            ARAnchor worldAnchor = spawnedPortal.AddComponent<ARAnchor>();
            currentAnchor = worldAnchor; // Store reference
        }

        portalSpawned = true; // ✅ Portal placed

        Debug.Log("✅ Portal placed! Stopping plane detection...");

        // ✅ Stop detecting new planes and hide existing ones
        planeManager.enabled = false;
        foreach (var detectedPlane in planeManager.trackables)
        {
            detectedPlane.gameObject.SetActive(false);
        }

        // Find the portalPlane inside the spawned PORTAL prefab
        Transform portalPlane = spawnedPortal.transform.Find("Portal blue/portalPlane");

        if (portalPlane != null)
        {
            PortalManager portalManager = portalPlane.GetComponent<PortalManager>();

            if (portalManager != null)
            {
                portalManager.MainCamera = Camera.main.gameObject;
                Debug.Log("✅ Portal configured successfully!");
            }
            else
            {
                Debug.LogError("❌ PortalManager component not found on portalPlane!");
            }
        }
        else
        {
            Debug.LogError("❌ portalPlane not found inside the PORTAL prefab!");
        }
    }


    void OnDestroy()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }
}
