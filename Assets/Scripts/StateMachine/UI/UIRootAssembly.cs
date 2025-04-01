using UnityEngine;

/// <summary>
/// UI Root class, used for storing references to UI views.
/// </summary>
public class UIRootAssembly : MonoBehaviour
{
    public AssemblyView AssemblyView;
    public AssemblyViewStepBase AssemblyViewStepBase;
    public AssemblyViewStepRing AssemblyViewStepRing;

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

        //DontDestroyOnLoad(this.gameObject);
    }

}
