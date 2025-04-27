using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepWheelPlanet2 : BaseViewSingleton<AssemblyViewStepWheelPlanet2>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;

    // public GameObject InstructionsCanvas;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    
    public GameObject planet2;

    public void ShowViewWithMesh() 
    {
    }

    public void OnEnable()
    {
        planet2.SetActive(true);
    }

    public void OnDisable()
    {
        planet2.SetActive(false);
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