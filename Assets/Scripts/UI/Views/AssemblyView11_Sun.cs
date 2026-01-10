using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyView11_Sun : BaseViewSingleton<AssemblyView11_Sun>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject sun;
    
    private const string Title = 
        "Schritt 2: Sonnenrad";
    private const string Description = 
        "Setzen Sie das Sonnenrad (das größte Rad) mit dem Gewinde nach unten in das zentrale Loch des Gehäuseunterteils ein.";

    public void OnEnable()
    {
        if (sun != null)
            sun.SetActive(true);
        
        InitializeMenu(Title, Description);
    }
    public void OnDisable()
    {
        if (sun != null)
            sun.SetActive(false);
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