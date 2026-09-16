using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;

public class ARObjectLightController : MonoBehaviour
{
    public HDRLightEstimation lightEstimation; // Reference to HDRLightEstimation script

    public GameObject lampBulb;   // Assign the bulb part of the lamp in Inspector
    public Light lampLight;       // Assign a Light component inside the lamp
    public Renderer laptopScreen; // Assign the laptop screen material in Inspector

    public Light laptopScreenLight; // Assign the Area Light in Inspector
    public float minLightIntensity = 1f; // Minimum brightness
    public float maxLightIntensity = 5f; // Maximum brightness
    private float brightnessThreshold = 0.7f; // Adjust this to control when the light increases

    private void Update()
    {
        if (lightEstimation != null && lightEstimation.brightness.HasValue)
        {
            UpdateObjectLighting();
        }
    }

    private void UpdateObjectLighting()
    {
        float brightness = lightEstimation.brightness.Value;

        if (brightness > brightnessThreshold)
        {
            //  Bright environment: Turn off lamp and dim laptop
            if (lampLight) lampLight.enabled = false;
            if (lampBulb) lampBulb.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

            if (laptopScreenLight)
            {
                laptopScreenLight.intensity = Mathf.Lerp(laptopScreenLight.intensity, minLightIntensity, Time.deltaTime * 2);
            }
        }
        else
        {
            // Dark environment: Turn on lamp and brighten laptop
            if (lampLight) lampLight.enabled = true;
            if (lampBulb) lampBulb.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

            if (laptopScreenLight)
            {
                laptopScreenLight.intensity = Mathf.Lerp(laptopScreenLight.intensity, maxLightIntensity, Time.deltaTime * 2);
            }
        }
    }
}
