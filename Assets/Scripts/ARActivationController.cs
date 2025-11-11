using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARActivationController : MonoBehaviour
{
    [SerializeField] private ARCameraBackground cameraBackground;
    [SerializeField] private Camera arCamera;
    
    // [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    
    // [SerializeField] private ARPointCloudManager pointCloudManager;
    // [SerializeField] private ARAnchorManager anchorManager;
    //[SerializeField] private ARSession; // keep this ENABLED

    public void PauseAR()
    {
        // arCamera.enabled = false;
        
        // cameraBackground.enabled = false;
        
        // planeManager.enabled = false;
        trackedImageManager.enabled = false;
        
        //pointCloudManager.enabled = false;
        //anchorManager.enabled = false;
        // arSession stays enabled, so camera tracking continues.
    }

    public void ResumeAR()
    {
        
        // arCamera.enabled = true;
        
        // cameraBackground.enabled = true;
        
        // planeManager.enabled = true;
        trackedImageManager.enabled = true;
        
        //pointCloudManager.enabled = true;
        //anchorManager.enabled = true;
    }
}
