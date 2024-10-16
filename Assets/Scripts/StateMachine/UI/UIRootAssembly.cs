using UnityEngine;

/// <summary>
/// UI Root class, used for storing references to UI views.
/// </summary>
public class UIRootAssembly : MonoBehaviour
{
    [SerializeField]
    private AssemblyView assemblyView;
    public AssemblyView AssemblyView => assemblyView;


    public void Awake()
    {
        Debug.Log("[UIRootAssembly.cs] Awaking.");

        // Required for proper creation of GameObjects independent
        // of their activation state before play mode.
        AssemblyView.gameObject.SetActive(false);
        AssemblyView.gameObject.SetActive(true);

        AssemblyView.Instance.HideView();
    }

}
