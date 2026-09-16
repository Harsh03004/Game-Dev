using UnityEngine;
using Vuforia;
using System.Collections;

public class GameStartController : MonoBehaviour
{
    public AnimationClip boxAnimationClip;   // Assign "Box Opening" Animation Clip
    public AnimationClip spaceshipAnimationClip; // Assign "UP" Animation Clip
    public GameObject boxObject;  // Assign the Box GameObject in Inspector
    public GameObject spaceshipObject; // Assign the Spaceship GameObject in Inspector
    public GameObject gameElements; // Game elements to activate

    private ObserverBehaviour observerBehaviour;
    private bool hasStarted = false;

    void Start()
    {
        gameElements.SetActive(false);  // Hide game elements initially
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        else
        {
            Debug.LogError("ObserverBehaviour component is missing! Attach this script to the ImageTarget.");
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (!hasStarted && (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED))
        {
            hasStarted = true;  // Ensure animations play only once
            StartCoroutine(PlayAnimations());
        }
    }

    IEnumerator PlayAnimations()
    {
        // Play Box Animation
        if (boxObject != null && boxAnimationClip != null)
        {
            Animation anim = boxObject.AddComponent<Animation>();
            anim.AddClip(boxAnimationClip, "Box");
            anim.Play("Box");
            yield return new WaitUntil(() => !anim.isPlaying);
        }
        else
        {
            Debug.LogError("Box object or animation clip is missing!");
        }

        // Play Spaceship Animation
        if (spaceshipObject != null && spaceshipAnimationClip != null)
        {
            Animation anim = spaceshipObject.AddComponent<Animation>();
            anim.AddClip(spaceshipAnimationClip, "UP");
            anim.Play("UP");
            yield return new WaitUntil(() => !anim.isPlaying);
        }
        else
        {
            Debug.LogError("Spaceship object or animation clip is missing!");
        }

        // Enable game elements after animations finish
        gameElements.SetActive(true);
        Debug.Log("Game Started!");
    }
}
