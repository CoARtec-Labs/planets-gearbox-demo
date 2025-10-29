using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepCarrier : BaseViewSingleton<AssemblyViewStepCarrier>
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
    
    public GameObject carrier;

    private const string Title = "Schritt 5: Abtriebsrad";
    private const string Description = 
        "Setzen Sie das Abtriebsrad mittig über der Sun ein, do dass die Stifte in alle drei Zahnräder greifen.";

    public void OnEnable()
    {
        if (carrier != null)
            carrier.SetActive(true);
        
        InitializeMenu(Title, Description);
    }

    public void OnDisable()
    {
        if (carrier != null)
            carrier.SetActive(false);
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