using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Adjust the speed as needed
    public AudioSource asource;
    public AudioClip aclip;
    private int counter;
    public Text cointext;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        // Get input from arrow keys or WASD for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Move the player in the calculated direction
        transform.Translate(movement * speed * Time.deltaTime);
    }
}