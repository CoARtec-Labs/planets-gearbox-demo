using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Script to pause and resume AR engine components.
/// </summary>
public class ARActivationController : MonoBehaviour
{
    [SerializeField] private ARCameraBackground cameraBackground;
    [SerializeField] private Camera playerCamera;
    
    // [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARTrackedImageManager imageTracker;
    // [SerializeField] private ARPointCloudManager pointCloudManager;
    // [SerializeField] private ARAnchorManager anchorManager;
    
    // [SerializeField] private arSession; // keep this ENABLED

    public void PauseAR()
    {
        // arCamera.enabled = false;
        // cameraBackground.enabled = false;
        
        // planeManager.enabled = false;
        imageTracker.enabled = false;
        // pointCloudManager.enabled = false;
        // anchorManager.enabled = false;
        
        // arSession stays enabled, so camera tracking continues.
    }

    public void ResumeAR()
    {
        
        // arCamera.enabled = true;
        // cameraBackground.enabled = true;
        
        // planeManager.enabled = true;
        imageTracker.enabled = true;
        //pointCloudManager.enabled = true;
        //anchorManager.enabled = true;
    }
}
