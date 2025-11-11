using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// UI Root class, used for storing references to UI views.
/// </summary>
public class UIRootAssembly : UIRootSingleton<UIRootAssembly>
{
    
    public List<GameObject> sceneObjects = new List<GameObject>();

    [SerializeField] private ARActivationController arController;
    [SerializeField] public bool controlSceneObjects = true;

    /// <summary>
    /// Return array of all (including inactive) views located as children under this UI root object.
    /// </summary>
    private BaseView[] GetAllViews()
    {
        return transform.GetComponentsInChildren<BaseView>(true);
    }

    private void InitializeViews()
    {
        Debug.Log("[UIRootAssembly.cs] Activating (initializing) views.");
        // This procedure is required to make sure all views' "Instances" get initialized.
        
        List<BaseView> views = GetAllViews().ToList();

        views.ForEach(view => view.gameObject.SetActive(false));
        views.ForEach(view => view.gameObject.SetActive(true));
        views.ForEach(view => view.HideView());
    }

    public void ActivateSceneObjects()
    {
        Debug.Log("[UIRootAssembly.cs] Activating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(true));
        
        arController.ResumeAR();
    }
    
    public void DeactivateSceneObjects()
    {
        Debug.Log("[UIRootAssembly.cs] Deactivating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(false));
        
        arController.PauseAR();
    }
    
    private new void Awake()
    {
        base.Awake();
        
        Debug.Log("[UIRootAssembly.cs] Awaking.");

        if (controlSceneObjects)
        {
            InitializeViews();
            // DeactivateSceneObjects();
        }
        else
        {
            DeactivateSceneObjects();
            ActivateSceneObjects();
        }

        //DontDestroyOnLoad(this.gameObject);
    }

}
