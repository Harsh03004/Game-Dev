using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    float pitch = 0f;
    float yaw = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Hides and locks the cursor
    }

    void Update()
    {
        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limit up/down rotation

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // WASD Movement
        float horizontal = Input.GetAxis("Horizontal"); // A and D
        float vertical = Input.GetAxis("Vertical");     // W and S

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}
