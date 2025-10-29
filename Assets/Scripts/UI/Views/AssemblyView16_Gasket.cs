using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyView16_Gasket : BaseViewSingleton<AssemblyView16_Gasket>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject gasket;
    
    private const string Title = "Schritt 6: Dichtung";
    private const string Description = 
        "Setzen Sie die Dichtung vorsichtig auf den Flansch des Gehäuseunterteils.";

    public void OnEnable()
    {
        if (gasket != null)
            gasket.SetActive(true);
        
        InitializeMenu(Title, Description);
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