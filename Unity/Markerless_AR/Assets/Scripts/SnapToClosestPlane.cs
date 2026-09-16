using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SnapToClosestPlane : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public GameObject prefab;

    // Update is called once per frame
    void Update()
    {
        if (planeManager.trackables.count == 0) return;

        ARPlane closestPlane = null;

        float minY=float.MaxValue;

        foreach(ARPlane plane in planeManager.trackables)
        {
            if(plane.transform.position.y<minY)
            {
                minY=plane.transform.position.y;
                closestPlane = plane;
            }
        }

        if(closestPlane != null) {
            prefab.transform.position=new Vector3(
                prefab.transform.position.x,
                closestPlane.transform.position.y,
                prefab.transform.position.z);
        }
    }
}
