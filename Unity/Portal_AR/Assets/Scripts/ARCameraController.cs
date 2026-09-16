using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCameraController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 100.0f;
    private ARCameraManager arCameraManager;
    private Camera arCamera;

    void Start()
    {
        arCameraManager = FindObjectOfType<ARCameraManager>();
        if (arCameraManager != null)
        {
            arCamera = arCameraManager.GetComponent<Camera>();
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        //HandleEditorMovement();
#else
            FollowDeviceCamera();
#endif
    }

    //void HandleEditorMovement()
    //{
    //    float moveX = Input.GetAxis("Horizontal"); // A, D
    //    float moveZ = Input.GetAxis("Vertical");   // W, S

    //    Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
    //    transform.position += transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;

    //    float rotation = 0;
    //    if (Input.GetKey(KeyCode.Q)) rotation = -rotationSpeed * Time.deltaTime; // Rotate Left
    //    if (Input.GetKey(KeyCode.E)) rotation = rotationSpeed * Time.deltaTime;  // Rotate Right

    //    transform.Rotate(0, rotation, 0);
    //}

    void FollowDeviceCamera()
    {
        if (arCamera != null)
        {
            transform.position = arCamera.transform.position;
            transform.rotation = arCamera.transform.rotation;
        }
    }
}