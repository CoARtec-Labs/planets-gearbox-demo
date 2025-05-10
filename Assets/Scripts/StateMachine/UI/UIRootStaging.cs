using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI Root class, used for storing references to UI views.
/// </summary>
public class UIRootStaging : UIRootSingleton<UIRootStaging>
{
    [SerializeField]
    private StagingView stagingView;
    public StagingView StagingView => stagingView;
    
    public List<GameObject> sceneObjects = new List<GameObject>();

    public bool activateScene = false;

    private void Update()
    {
        // Trigger scene objects activation
        if (activateScene)
        {
            ActivateSceneObjects();
            StagingView.Instance.ShowView();
            activateScene = false;
        }
    }
    
    public void ActivateSceneObjects()
    {
        Debug.Log("[UIRootStaging.cs] Activating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(true));
    }
    
    public void DeactivateSceneObjects()
    {
        Debug.Log("[UIRootStaging.cs] Deactivating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(false));
    }
    
    public new void Awake()
    {
        base.Awake(); // Initialize singleton
        
        DeactivateSceneObjects();
        
        StagingView.gameObject.SetActive(true);
        StagingView.Instance.HideView();
    }

}
