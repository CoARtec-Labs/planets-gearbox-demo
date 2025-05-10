using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// UI Root class, used for storing references to UI views.
/// </summary>
public class UIRootAssembly : UIRootSingleton<UIRootAssembly>
{
    // public AssemblyView AssemblyView;
    // public AssemblyViewStepBase AssemblyViewStepBase;
    // public AssemblyViewStepRing AssemblyViewStepRing;
    // public AssemblyViewStepWheelSun AssemblyViewStepWheelSun;
    // public AssemblyViewStepWheelPlanet1 AssemblyViewStepWheelPlanet1;
    // public AssemblyViewStepWheelPlanet2 AssemblyViewStepWheelPlanet2;
    // public AssemblyViewStepWheelPlanet3 AssemblyViewStepWheelPlanet3;
    // public AssemblyViewStepCarrier AssemblyViewStepCarrier;
    // public AssemblyViewStepGasket AssemblyViewStepGasket;
    // public AssemblyViewStepLid AssemblyViewStepLid;
    
    public List<GameObject> sceneObjects = new List<GameObject>();

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
        // This procedure is required to make sure all view's 'Instances' get initialized.
        
        List<BaseView> views = GetAllViews().ToList();

        views.ForEach(view => view.gameObject.SetActive(false));
        views.ForEach(view => view.gameObject.SetActive(true));
        views.ForEach(view => view.HideView());
    }

    public void ActivateSceneObjects()
    {
        Debug.Log("[UIRootAssembly.cs] Activating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(true));
    }
    
    public void DeactivateSceneObjects()
    {
        Debug.Log("[UIRootAssembly.cs] Deactivating relevant scene objects.");
        
        sceneObjects.ForEach(item => item.gameObject.SetActive(false));
    }
    
    private new void Awake()
    {
        base.Awake();
        
        Debug.Log("[UIRootAssembly.cs] Awaking.");

        
        InitializeViews();
        DeactivateSceneObjects();

        
        // Required for proper creation of GameObjects independent
        // of their activation state before play mode.
        // AssemblyView.gameObject.SetActive(false);
        // AssemblyView.gameObject.SetActive(true);
        // AssemblyView.Instance.HideView();

        // AssemblyViewStepBase.gameObject.SetActive(false);
        // AssemblyViewStepBase.gameObject.SetActive(true);
        // AssemblyViewStepBase.HideView();

        /*
        AssemblyViewStepRing.gameObject.SetActive(false);
        AssemblyViewStepRing.gameObject.SetActive(true);
        AssemblyViewStepRing.Instance.HideView();

        AssemblyViewStepWheelSun.gameObject.SetActive(false);
        AssemblyViewStepWheelSun.gameObject.SetActive(true);
        AssemblyViewStepWheelSun.HideView();

        AssemblyViewStepWheelPlanet1.gameObject.SetActive(false);
        AssemblyViewStepWheelPlanet1.gameObject.SetActive(true);
        AssemblyViewStepWheelPlanet1.HideView();

        AssemblyViewStepWheelPlanet2.gameObject.SetActive(false);
        AssemblyViewStepWheelPlanet2.gameObject.SetActive(true);
        AssemblyViewStepWheelPlanet2.HideView();

        AssemblyViewStepWheelPlanet3.gameObject.SetActive(false);
        AssemblyViewStepWheelPlanet3.gameObject.SetActive(true);
        AssemblyViewStepWheelPlanet3.HideView();

        AssemblyViewStepCarrier.gameObject.SetActive(false);
        AssemblyViewStepCarrier.gameObject.SetActive(true);
        AssemblyViewStepCarrier.HideView();

        AssemblyViewStepGasket.gameObject.SetActive(false);
        AssemblyViewStepGasket.gameObject.SetActive(true);
        AssemblyViewStepGasket.HideView();

        AssemblyViewStepLid.gameObject.SetActive(false);
        AssemblyViewStepLid.gameObject.SetActive(true);
        AssemblyViewStepLid.HideView();
        */

        //DontDestroyOnLoad(this.gameObject);
    }

}
