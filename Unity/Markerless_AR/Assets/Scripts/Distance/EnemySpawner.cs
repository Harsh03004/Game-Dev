using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;  // Assign Enemy from the Hierarchy (Not a prefab)
    public GameObject nearCirclePrefab, farCirclePrefab; // Prefabs for circles
    public Material redMaterial, greenMaterial;

    private GameObject nearCircle, farCircle, spawnedEnemy;
    private ARPlaneManager planeManager;
    private Camera cam;
    private bool enemySpawnedNear = false;

    private void Start()
    {
        cam = Camera.main;
        planeManager = FindObjectOfType<ARPlaneManager>();

        if (planeManager != null)
        {
            planeManager.planesChanged += OnPlanesChanged;
        }
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added.Count == 0) return; // No new planes detected

        List<ARPlane> planes = new List<ARPlane>();

        foreach (ARPlane plane in planeManager.trackables)
        {
            planes.Add(plane);
        }

        ARPlane lowestPlane = FindLowestPlane(planes);
        if (lowestPlane != null)
        {
            SpawnCirclesOnPlane(lowestPlane);
            planeManager.planesChanged -= OnPlanesChanged; // Stop detecting planes after spawning circles
        }
    }

    private ARPlane FindLowestPlane(List<ARPlane> planes)
    {
        ARPlane lowestPlane = null;
        float minY = float.MaxValue;

        foreach (ARPlane plane in planes)
        {
            if (plane.transform.position.y < minY)
            {
                minY = plane.transform.position.y;
                lowestPlane = plane;
            }
        }
        return lowestPlane;
    }

    void SpawnCirclesOnPlane(ARPlane plane)
    {
        Vector3 center = plane.center;
        Vector3 nearPosition = center + new Vector3(0, 0.01f, 3f); // Adjust offsets
        Vector3 farPosition = center + new Vector3(0, 0.01f, 6f);

        nearCircle = Instantiate(nearCirclePrefab, nearPosition, Quaternion.identity);
        farCircle = Instantiate(farCirclePrefab, farPosition, Quaternion.identity);

        SetCircleColors(Color.white, Color.white);
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && nearCircle != null && farCircle != null)
        {
            Vector3 tapPosition = GetTapPosition();
            TrySpawnEnemy(tapPosition);
        }

        UpdateCircleColors();
    }

    Vector3 GetTapPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            return Input.mousePosition;
        }
        return Vector3.zero;
    }

    void TrySpawnEnemy(Vector3 tapPosition)
    {
        Ray ray = cam.ScreenPointToRay(tapPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == nearCircle)
            {
                SpawnEnemy(nearCircle.transform.position);
                enemySpawnedNear = true;
                SetCircleColors(Color.red, Color.green);
            }
            else if (hit.collider.gameObject == farCircle)
            {
                SpawnEnemy(farCircle.transform.position);
                enemySpawnedNear = false;
                SetCircleColors(Color.green, Color.green); // Far circle starts green
            }
        }
    }

    void SpawnEnemy(Vector3 spawnPosition)
    {
        if (spawnedEnemy != null) return; // Only one enemy at a time

        spawnedEnemy = enemy;  // Use the enemy from the hierarchy
        spawnedEnemy.transform.position = spawnPosition;
        spawnedEnemy.SetActive(true);
    }

    void UpdateCircleColors()
    {
        if (spawnedEnemy == null) return;

        float playerToEnemyDist = Vector3.Distance(cam.transform.position, spawnedEnemy.transform.position);

        if (enemySpawnedNear)
        {
            SetCircleColors(Color.red, Color.green); // Near enemy: Near = red, Far = green
        }
        else
        {
            // **NEW: Change far circle based on distance**
            if (playerToEnemyDist > 3f)
            {
                SetCircleColors(Color.green, Color.green); // Player far: Keep both green
            }
            else
            {
                SetCircleColors(Color.green, Color.red); // Player close (≤2f): Make far circle red
            }
        }
    }

    void SetCircleColors(Color nearColor, Color farColor)
    {
        if (nearCircle != null) nearCircle.GetComponent<Renderer>().material.color = nearColor;
        if (farCircle != null) farCircle.GetComponent<Renderer>().material.color = farColor;
    }
}
