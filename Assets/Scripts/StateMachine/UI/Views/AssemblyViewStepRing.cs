using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepRing : BaseViewSingleton<AssemblyViewStepRing>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;

    // public GameObject InstructionsCanvas;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    
    [Header("Mesh Controller")]
    public AssemblyMeshController meshController;  // Add this

    public void ShowViewWithMesh() 
    {
        ShowView();
        Debug.Log($"Mesh Controller Assigned: {meshController != null}");
        if (meshController != null) 
        {
            Debug.Log($"Attempting to show ring mesh: {meshController.ring != null}");
            meshController.ShowOnly(meshController.ring);
        }
    }
    
    public void NextClick()
    {
        OnNextClicked?.Invoke();
    }

    public void BackClick()
    {
        OnBackClicked?.Invoke();
    }

}
