using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace coartec.detection
{
    /// <summary>
    /// Script for activation of camera or webcam updater depending on untity run enverionment.
    /// </summary>
    public class ImageUpdaterManager : MonoBehaviour
    {

        [SerializeField] private CameraImageUpdater cameraUpdater;
        [SerializeField] private WebcamImageUpdater webcamUpdater;

        [SerializeField] private DetectionManager detectionManager;
        
        void OnEnable()
        {
            // Activate branch of game objects conditionally
#if UNITY_EDITOR
            // Editor mode uses webcam of a desktop machine
            cameraUpdater.gameObject.transform.parent.gameObject.SetActive(false);
            webcamUpdater.gameObject.transform.parent.gameObject.SetActive(true);
#else
            // Device mode uses camera onject of the AR engine (AR camera manager)
            cameraUpdater.gameObject.transform.parent.gameObject.SetActive(false);
            webcamUpdater.gameObject.transform.parent.gameObject.SetActive(true);
#endif
        }

        private void Start()
        {
            // Link respective image updater to the detection manager
#if UNITY_EDITOR    
            // Editor mode
            detectionManager.SetImageUpdater(webcamUpdater);
#else 
            // Device mode
            detectionManager.setImageUpdater(cameraUpdater);
#endif            
        }
    }
}