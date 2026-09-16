using UnityEngine;

public class LampController : MonoBehaviour
{
    private Light lampLight;
    private bool isLightOn = false;

    private void Start()
    {
        // Find the Light component inside the lamp
        lampLight = GetComponentInChildren<Light>();

        if (lampLight != null)
        {
            lampLight.enabled = false; // Start with light OFF
            lampLight.shadows = LightShadows.Soft; // Enable soft shadows
        }
        else
        {
            Debug.LogError("No Light component found in the lamp!");
        }
    }

    public void ToggleLight()
    {
        if (lampLight != null)
        {
            isLightOn = !isLightOn;
            lampLight.enabled = isLightOn;
            Debug.Log("Lamp Light Toggled: " + isLightOn);
        }
    }
}
