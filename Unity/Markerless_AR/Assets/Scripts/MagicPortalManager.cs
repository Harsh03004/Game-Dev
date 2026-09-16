using UnityEngine;
using UnityEngine.UI;

public class MagicPortalManager : MonoBehaviour
{
    public GameObject[] magicPrefabs; // Array of magic prefabs
    public Transform spawnPoint;      // Spawn location for portals
    public Button nextButton;         // Button for going to the next portal
    public Button backButton;         // Button for going to the previous portal
    public Button startButton;        // Start button to activate portal navigation

    private int currentIndex = 0;
    private GameObject currentPortal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initially deactivate portal buttons
        nextButton.interactable = false;
        backButton.interactable = false;

        // Add listener for the start button
        startButton.onClick.AddListener(OnStartButton);
    }

    public void OnNextButton()
    {
        if (magicPrefabs.Length == 0) return;

        currentIndex = (currentIndex + 1) % magicPrefabs.Length;
        SpawnPortal(currentIndex);
    }


    public void OnBackButton()
    {
        if (magicPrefabs.Length == 0) return;

        currentIndex = (currentIndex - 1 + magicPrefabs.Length) % magicPrefabs.Length;
        SpawnPortal(currentIndex);
    }

    private void OnStartButton()
    {
        // Enable the portal buttons
        nextButton.interactable = true;
        backButton.interactable = true;

        // Optionally hide or disable the start button after clicking
        startButton.interactable = false;
        startButton.gameObject.SetActive(false);

        // Spawn the first portal
        if (magicPrefabs.Length > 0)
        {
            SpawnPortal(currentIndex);
        }
    }

    private void SpawnPortal(int index)
    {
        // Destroy the existing portal if it exists
        if (currentPortal != null)
        {
            Destroy(currentPortal);
        }

        // Instantiate the new portal
        currentPortal = Instantiate(magicPrefabs[index], spawnPoint.position, spawnPoint.rotation);
    }
}
