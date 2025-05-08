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
    
    public DemoScreenManager DemoScreenManager;

    public List<GameObject> sceneObjects = new List<GameObject>();
    
    public void ActivateSceneObjects()
    {
        Debug.Log("[UIRootStaging.cs] Activating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(true));

        DemoScreenManager.UpdateDisplayManually();
    }
    
    public void DeactivateSceneObjects()
    {
        Debug.Log("[UIRootStaging.cs] Deactivating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(false));
    }
    
    public void Awake()
    {
        base.Awake(); // Initialize singleton
        
        StagingView.gameObject.SetActive(true);
        StagingView.Instance.HideView();
    }

}
