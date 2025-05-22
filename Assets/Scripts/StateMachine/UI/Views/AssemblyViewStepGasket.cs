using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepGasket : BaseViewSingleton<AssemblyViewStepGasket>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject gasket;
    
    public void OnEnable()
    {
        if (gasket != null)
            gasket.SetActive(true);
    }

    public void OnDisable()
    {
        if (gasket != null)
            gasket.SetActive(false);
    }
    
    public void NextClick()
    {
        OnNextClicked?.Invoke();
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