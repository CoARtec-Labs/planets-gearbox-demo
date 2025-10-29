using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// UI Root class, used for storing references to UI views.
/// </summary>
public class UIRootDetection : UIRootSingleton<UIRootDetection>
{
    [FormerlySerializedAs("stagingView")] [SerializeField]
    private DetectionView detectionView;
    public DetectionView DetectionView => detectionView;
    
    public List<GameObject> sceneObjects = new List<GameObject>();

    public bool activateScene = false;

    private void Update()
    {
        // Trigger scene objects activation
        if (activateScene)
        {
            ActivateSceneObjects();
            DetectionView.Instance.ShowView();
            activateScene = false;
        }
    }
    
    public void ActivateSceneObjects()
    {
        Debug.Log("[UIRootDetection.cs] Activating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(true));
    }
    
    public void DeactivateSceneObjects()
    {
        Debug.Log("[UIRootDetection.cs] Deactivating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(false));
    }
    
    public new void Awake()
    {
        base.Awake(); // Initialize singleton
        
        DeactivateSceneObjects();
        
        DetectionView.gameObject.SetActive(true);
        DetectionView.Instance.HideView();
    }

}
