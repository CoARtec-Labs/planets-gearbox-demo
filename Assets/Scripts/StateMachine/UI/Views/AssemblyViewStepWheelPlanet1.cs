using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepWheelPlanet1 : BaseViewSingleton<AssemblyViewStepWheelPlanet1>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject planet1;

    public void ShowViewWithMesh() 
    {
    }

    public void OnEnable()
    {
        if (planet1 != null)
            planet1.SetActive(true);
    }

    public void OnDisable()
    {
        if (planet1 != null)
            planet1.SetActive(false);
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