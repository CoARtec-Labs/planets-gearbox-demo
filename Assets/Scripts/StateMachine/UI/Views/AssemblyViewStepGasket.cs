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

    // public GameObject InstructionsCanvas;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    
    public GameObject gasket;

    public void ShowViewWithMesh() 
    {
    }

    public void OnEnable()
    {
        gasket.SetActive(true);
    }

    public void OnDisable()
    {
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

}