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
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject planet2;

    private const string Title = "Planet 2";
    private const string Description = 
        "Platzieren Sie das zweite Zahnrad auf der linken oberen Seite der Sun.";

    public void OnEnable()
    {
        // TODO Add reference-destroyed checks ( == null) here and at other Views.
        planet2.SetActive(true);
        
        InitializeMenu(Title, Description);
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
    
    public void StagingClick()
    {
        OnStagingClicked?.Invoke();
    }
}