using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PortalManager : MonoBehaviour
{
    public GameObject MainCamera; // Reference to the AR Camera (assigned dynamically)
    public GameObject station; // The environment inside the portal
    public GameObject portalPlane; // The portal plane
    private List<Material> stationMaterials = new List<Material>(); // Materials for stencil effect

    void Start()
    {
        // ✅ Find the AR Camera under XR Origin dynamically
        if (MainCamera == null)
        {
            MainCamera = Camera.main?.gameObject;
        }

        if (MainCamera == null)
        {
            Debug.LogError("❌ AR Camera (Main Camera) not found! Make sure the camera is tagged as 'MainCamera'.");
            return;
        }

        // ✅ Ensure the portalPlane has a collider & rigidbody
        Collider portalCollider = portalPlane.GetComponent<Collider>();
        if (portalCollider == null) {
            portalCollider = portalPlane.AddComponent<BoxCollider>();
        }
        portalCollider.isTrigger = true;

        Rigidbody portalRigidbody = portalPlane.GetComponent<Rigidbody>();
        if (portalRigidbody == null) {
            portalRigidbody = portalPlane.AddComponent<Rigidbody>();
        }
        portalRigidbody.isKinematic = true;
        portalRigidbody.useGravity = false;

        // ✅ Ensure the AR camera has a collider & rigidbody for portal detection
        Collider cameraCollider = MainCamera.GetComponent<Collider>();
        if (cameraCollider == null) {
            cameraCollider = MainCamera.AddComponent<SphereCollider>();
        }
        //cameraCollider.isTrigger = true;

        Rigidbody cameraRigidbody = MainCamera.GetComponent<Rigidbody>();
        if (cameraRigidbody == null) {
            cameraRigidbody = MainCamera.AddComponent<Rigidbody>();
        }
        cameraRigidbody.isKinematic = true;
        cameraRigidbody.useGravity = false;

        // ✅ Get all materials from the station (for stencil effect)
        Renderer[] childRenderers = station.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers) {
            stationMaterials.AddRange(renderer.materials);
        }
    }
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject == MainCamera)
        {
            Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);
            int stencilValue = camPositionInPortalSpace.z < 0.0f ? (int)CompareFunction.Always : (int)CompareFunction.Equal;

           // Debug.Log($"🔄 Updating Stencil: {stencilValue}, Camera Position: {camPositionInPortalSpace}");

            foreach (Material mat in stationMaterials)
            {
                mat.SetInt("_StencilComp", stencilValue);
            }
        }
    }



    void OnTriggerExit(Collider collider)
    {
        // ✅ Reset the stencil effect when the AR Camera exits the portal
        if (collider.gameObject == MainCamera) {
            foreach (Material mat in stationMaterials) {
                mat.SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
        }
    }
}
