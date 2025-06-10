using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepLid : BaseViewSingleton<AssemblyViewStepLid>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    
    public GameObject lid;

    private const string Title = "Lid";
    private const string Description = 
        "Schließen Sie das Gehäuse mit dem Deckel (Lid).";

    public void OnEnable()
    {
        if (lid != null)
            lid.SetActive(true);
        
        InitializeMenu(Title, Description);
    }

    public void OnDisable()
    {
        if (lid != null)
            lid.SetActive(false);
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