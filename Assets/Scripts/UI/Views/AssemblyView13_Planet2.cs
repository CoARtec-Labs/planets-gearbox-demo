using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyView13_Planet2 : BaseViewSingleton<AssemblyView13_Planet2>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject planet2;

    private const string Title = 
        "Schritt 4: schwarzes Zahnrad";
    private const string Description = 
        "Platzieren Sie das schwarze Zahnrad auf der linken oberen Seite des Sonnenrads.";

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