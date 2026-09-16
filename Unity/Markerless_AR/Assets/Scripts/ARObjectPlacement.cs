using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ARObjectPlacement : MonoBehaviour
{
    public GameObject objectPrefab; // The object to place
    public ARRaycastManager raycastManager;
    public Button spawnButton;
    public Button confirmButton; // Button to finalize placement

    private GameObject spawnedObject;
    private bool isPlacing = false;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 initialTouchPos; // For rotation
    private float initialPinchDistance; // For scaling
    private Vector3 initialScale;
    private bool isRotating = false;

    private void Start()
    {
        spawnButton.onClick.AddListener(OnSpawnButtonClicked);
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        confirmButton.gameObject.SetActive(false); // Hide confirm button initially
    }

    private void Update()
    {
        if (isPlacing && Input.touchCount > 0)
        {
            HandlePlacement(); // Move object freely
        }

        if (spawnedObject != null)
        {
            HandleRotation(); // Rotate with one finger (thumb swipe)
            HandleScaling(); // Scale with two-finger pinch
        }
    }

    // Click the button to spawn the object
    private void OnSpawnButtonClicked()
    {
        if (spawnedObject == null && raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            spawnedObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
            isPlacing = true; // Enable moving
            confirmButton.gameObject.SetActive(true); // Show confirm button
        }
    }

    // Move object freely with one finger
    private void HandlePlacement()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }

    // Rotate object with **one-finger thumb swipe**
    private void HandleRotation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialTouchPos = touch.position;
                isRotating = true;
            }
            else if (touch.phase == TouchPhase.Moved && isRotating)
            {
                float deltaX = touch.position.x - initialTouchPos.x; // Left/right swipe
                float rotationSpeed = 0.5f;
                spawnedObject.transform.Rotate(0, -deltaX * rotationSpeed, 0, Space.World);
                initialTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isRotating = false;
            }
        }
    }

    // Scale object with **two-finger pinch gesture**
    private void HandleScaling()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                initialScale = spawnedObject.transform.localScale;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                if (!Mathf.Approximately(initialPinchDistance, 0))
                {
                    float scaleFactor = currentPinchDistance / initialPinchDistance;
                    spawnedObject.transform.localScale = initialScale * scaleFactor;
                }
            }
        }
    }

    // Click confirm button to finalize placement
    private void OnConfirmButtonClicked()
    {
        isPlacing = false; // Stop moving the object
        confirmButton.gameObject.SetActive(false); // Hide confirm button
    }
}
