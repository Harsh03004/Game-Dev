using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ARTapToSpawn : MonoBehaviour
{
    public GameObject tablePrefab;
    public GameObject chairPrefab;
    public GameObject laptopPrefab;
    public GameObject studyLampPrefab;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    private GameObject selectedPrefab;
    private GameObject selectedObject;
    private GameObject spawnedTable;
    private GameObject spawnedLaptop;
    private bool isSpawning = false;
    private bool isDragging = false;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 initialTouchPosition;
    private float rotationSpeed = 100f;
    private float moveSpeed = 0.01f;

    public Button tableButton;
    public Button chairButton;
    public Button laptopButton;
    public Button lampButton;

    private void Start()
    {
        tableButton.onClick.AddListener(() => SelectObject(tablePrefab));
        chairButton.onClick.AddListener(() => SelectObject(chairPrefab));
        laptopButton.onClick.AddListener(() => SelectObject(laptopPrefab));
        lampButton.onClick.AddListener(() => SelectObject(studyLampPrefab));
    }

    private void Update()
    {
        if (isSpawning && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TrySpawnObject(Input.GetTouch(0).position);
        }

        if (isSpawning && Input.GetMouseButtonDown(0))
        {
            TrySpawnObject(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(0))
        {
            TrySelectObject(Input.mousePosition);
        }

        if (selectedObject != null)
        {
            HandleRotation();
            HandleDragging();
            HandleKeyboardMovement();
        }
    }

    private void SelectObject(GameObject prefab)
    {
        selectedPrefab = prefab;
        isSpawning = true;
    }

    private void TrySpawnObject(Vector2 position)
    {
        if (selectedPrefab != null && raycastManager.Raycast(position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (selectedPrefab == tablePrefab)
            {
                spawnedTable = Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);
                spawnedTable.tag = "SpawnedTable";
                spawnedTable.AddComponent<BoxCollider>();
            }
            else if (selectedPrefab == laptopPrefab && spawnedTable != null)
            {
                Vector3 tableTop = spawnedTable.transform.position + new Vector3(0, spawnedTable.transform.localScale.y / 2 + 0.05f, 0);
                spawnedLaptop = Instantiate(selectedPrefab, tableTop, spawnedTable.transform.rotation);
                spawnedLaptop.tag = "LaptopObject";
                spawnedLaptop.AddComponent<BoxCollider>();
            }
            else if (selectedPrefab == studyLampPrefab && spawnedTable != null)
            {
                Vector3 lampPosition = spawnedTable.transform.position + new Vector3(spawnedTable.transform.localScale.x / 2 - 0.4f, spawnedTable.transform.localScale.y / 2 + 0.18f, spawnedTable.transform.localScale.z / 2 - 0.1f);
                GameObject spawnedLamp = Instantiate(selectedPrefab, lampPosition, spawnedTable.transform.rotation);
                spawnedLamp.tag = "LampObject";
                spawnedLamp.AddComponent<BoxCollider>();
            }
            //else if (selectedPrefab == chairPrefab)
            //{
            //    GameObject spawnedChair = Instantiate(selectedPrefab, hitPose.position, Quaternion.Euler(0, hitPose.rotation.eulerAngles.y, 0));
            //    spawnedChair.tag = "ChairObject";
            //    spawnedChair.AddComponent<BoxCollider>();
            //}
            else
            {
                Debug.Log("Laptop and lamp can only be placed on a table!");
                return;
            }
            isSpawning = false;
            HideArPlane();
        }
    }

    private void TrySelectObject(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("LaptopObject"))
            {
                selectedObject = hit.transform.gameObject;
            }
        }
        else
        {
            selectedObject = null;
        }
    }

    private void HideArPlane()
    {
        if (planeManager != null)
        {
            planeManager.enabled = false;
        }
    }

    private void HandleRotation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float rotationAmount = -touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                selectedObject.transform.Rotate(0, rotationAmount, 0, Space.World);
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            selectedObject.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            selectedObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }
    }

    private void HandleDragging()
    {
        if (selectedObject == spawnedLaptop && spawnedTable != null)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 newPos = spawnedLaptop.transform.position + new Vector3(touch.deltaPosition.x * moveSpeed, 0, touch.deltaPosition.y * moveSpeed);

                    // Restrict movement within table boundaries
                    float halfTableWidth = spawnedTable.transform.localScale.x / 2;
                    float halfTableDepth = spawnedTable.transform.localScale.z / 2;
                    Vector3 tablePos = spawnedTable.transform.position;

                    newPos.x = Mathf.Clamp(newPos.x, tablePos.x - halfTableWidth + 0.1f, tablePos.x + halfTableWidth - 0.1f);
                    newPos.z = Mathf.Clamp(newPos.z, tablePos.z - halfTableDepth + 0.1f, tablePos.z + halfTableDepth - 0.1f);

                    spawnedLaptop.transform.position = newPos;
                }
            }
        }
    }

    private void HandleKeyboardMovement()
    {
        if (spawnedTable == null || spawnedLaptop == null)
            return;

        Vector3 newPos = spawnedLaptop.transform.position;

        if (Input.GetKey(KeyCode.O)) // Move forward
            newPos.z += moveSpeed;
        if (Input.GetKey(KeyCode.L)) // Move backward
            newPos.z -= moveSpeed;
        if (Input.GetKey(KeyCode.P)) // Move right
            newPos.x += moveSpeed;
        if (Input.GetKey(KeyCode.I)) // Move left
            newPos.x -= moveSpeed;

        // Restrict movement within table boundaries
        float halfTableWidth = spawnedTable.transform.localScale.x / 2;
        float halfTableDepth = spawnedTable.transform.localScale.z / 2;
        Vector3 tablePos = spawnedTable.transform.position;

        newPos.x = Mathf.Clamp(newPos.x, tablePos.x - halfTableWidth + 0.1f, tablePos.x + halfTableWidth - 0.1f);
        newPos.z = Mathf.Clamp(newPos.z, tablePos.z - halfTableDepth + 0.1f, tablePos.z + halfTableDepth - 0.1f);

        spawnedLaptop.transform.position = newPos;
    }
}
