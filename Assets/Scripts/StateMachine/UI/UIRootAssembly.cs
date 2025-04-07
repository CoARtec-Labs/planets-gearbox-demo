using UnityEngine;

/// <summary>
/// UI Root class, used for storing references to UI views.
/// </summary>
public class UIRootAssembly : MonoBehaviour
{
    public AssemblyView AssemblyView;
    public AssemblyViewStepBase AssemblyViewStepBase;
    public AssemblyViewStepRing AssemblyViewStepRing;
    public AssemblyViewStepWheelSun AssemblyViewStepWheelSun;
    public AssemblyViewStepWheelPlanet1 AssemblyViewStepWheelPlanet1;
    public AssemblyViewStepWheelPlanet2 AssemblyViewStepWheelPlanet2;
    public AssemblyViewStepWheelPlanet3 AssemblyViewStepWheelPlanet3;
    public AssemblyViewStepCarrier AssemblyViewStepCarrier;
    public AssemblyViewStepGasket AssemblyViewStepGasket;
    public AssemblyViewStepLid AssemblyViewStepLid;

    public void Awake()
    {
        Debug.Log("[UIRootAssembly.cs] Awaking.");

        // Required for proper creation of GameObjects independent
        // of their activation state before play mode.
        AssemblyView.gameObject.SetActive(false);
        AssemblyView.gameObject.SetActive(true);
        AssemblyView.Instance.HideView();

        AssemblyViewStepBase.gameObject.SetActive(false);
        AssemblyViewStepBase.gameObject.SetActive(true);
        AssemblyViewStepBase.Instance.HideView();

        AssemblyViewStepRing.gameObject.SetActive(false);
        AssemblyViewStepRing.gameObject.SetActive(true);
        AssemblyViewStepRing.HideView();
        
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
        

        //DontDestroyOnLoad(this.gameObject);
    }

}
