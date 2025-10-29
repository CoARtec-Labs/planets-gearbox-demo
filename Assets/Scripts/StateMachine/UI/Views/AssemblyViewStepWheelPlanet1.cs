using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
    
    [SerializeField] private GameObject planet1;

    private const string Title = "Schritt 3: weißes Zahnrad";
    private const string Description = 
        "Platzieren Sie das weiße Zahnrad auf der rechten oberen Seite des Sonnenrads.";

    public void OnEnable()
    {
        if (planet1 != null)
            planet1.SetActive(true);
        
        InitializeMenu(Title, Description);
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