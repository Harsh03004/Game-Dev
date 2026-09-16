using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARLightEstimation : MonoBehaviour
{
    public ARCameraManager arCameraManager;
    public Light directionalLight;

    void OnEnable()
    {
        if (arCameraManager != null)
            arCameraManager.frameReceived += OnLightFrameReceived;
    }

    void OnDisable()
    {
        if (arCameraManager != null)
            arCameraManager.frameReceived -= OnLightFrameReceived;
    }

    void OnLightFrameReceived(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            directionalLight.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
        }

        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
        {
            directionalLight.intensity = args.lightEstimation.mainLightIntensityLumens.Value / 1000f;
        }

        if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            RenderSettings.ambientProbe = args.lightEstimation.ambientSphericalHarmonics.Value;
        }
    }
}
