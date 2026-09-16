using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

public class PlanetClickHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            DetectClick(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Mobile touch
        {
            DetectClick(Input.GetTouch(0).position);
        }
    }

    void DetectClick(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                Debug.Log("Clicked on: " + hit.collider.gameObject.name);

                // Call OnClick() if the clicked object has a Planet script
                Planet planet = hit.collider.gameObject.GetComponent<Planet>();
                if (planet != null)
                {
                    planet.OnClick();
                }
            }
        }
    }
}
