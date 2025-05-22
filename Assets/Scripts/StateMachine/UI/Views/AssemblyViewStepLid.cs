using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepLid : BaseViewSingleton<AssemblyViewStepLid>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    
    public GameObject lid;

    public void ShowViewWithMesh() 
    {
    }

    public void OnEnable()
    {
        if (lid != null)
            lid.SetActive(true);
    }

    public void OnDisable()
    {
        if (lid != null)
            lid.SetActive(false);
    }
    public void BackClick()
    {
        OnBackClicked?.Invoke();
    }
    public void StagingClick()
    {
        OnStagingClicked?.Invoke();
    }

}