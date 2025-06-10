using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepWheelPlanet3 : BaseViewSingleton<AssemblyViewStepWheelPlanet3>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;
    
    // public GameObject InstructionsCanvas;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    
    public GameObject planet3;

    private const string Title = "Planet 3";
    private const string Description = 
        "Platzieren Sie das dritte Zahnrad in der Mitter unter der Sun.";
    
    public void OnEnable()
    {
        planet3.SetActive(true);
        
        InitializeMenu(Title, Description);
    }

    public void OnDisable()
    {
        planet3.SetActive(false);
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