using UnityEngine;

using Coartec.Detection;


/// <summary>
/// Script for activation of camera or webcam updater depending on unity run environment.
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
        // Device mode uses camera object of the AR engine (AR camera manager)
        cameraUpdater.gameObject.transform.parent.gameObject.SetActive(true);
        webcamUpdater.gameObject.transform.parent.gameObject.SetActive(false);
#endif
    }

    private void Start()
    {
        // Link respective image updater to the detection manager
#if UNITY_EDITOR    
        // Editor mode
        detectionManager.ImageUpdater = webcamUpdater;
#else
        // Device mode
        detectionManager.SetImageUpdater(cameraUpdater);
#endif
    }
}
