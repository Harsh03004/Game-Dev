using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject firstobject;  // The first object
    public GameObject secondobject; // The second object

    public string ColliderTagName;  // Tag for the collider

    public ParticleSystem glowEffect; // The glow effect

    public AudioClip evolutionSound; // Sound effect for evolution
    private AudioSource audioSource; // Audio source to play the sound

    private bool isDelayActive = false; // To track if the delay is active
    private float delayTimer = 0f;      // Timer for the delay
    private float delayDuration = 7f;  // Delay duration in seconds (changed to 7 seconds)

    void Start()
    {
        // Get or add an AudioSource component to the GameObject
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Check if the delay is active
        if (isDelayActive)
        {
            delayTimer += Time.deltaTime; // Increment the timer

            // Check if the timer has reached the delay duration
            if (delayTimer >= delayDuration)
            {
                secondobject.SetActive(true); // Activate the second object
           
                isDelayActive = false;       // Reset the delay state
                delayTimer = 0f;             // Reset the timer
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider matches and the first object is active
        if (other.CompareTag(ColliderTagName) && firstobject.activeSelf)
        {
            firstobject.SetActive(false); // Deactivate the first object
            glowEffect.Play();
            PlayEvolutionSound();
            isDelayActive = true;         // Start the delay
            delayTimer = 0f;              // Reset the timer
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Reset to the initial state when the collider exits
        if (other.CompareTag(ColliderTagName))
        {
            firstobject.SetActive(true);  // Reactivate the first object
            secondobject.SetActive(false); // Deactivate the second object
            isDelayActive = false;        // Cancel any ongoing delay
            delayTimer = 0f;              // Reset the timer
        }
    }

    private void PlayEvolutionSound()
    {
        if (evolutionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(evolutionSound);
        }
    }
}
