using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyView17_Lid : BaseViewSingleton<AssemblyView17_Lid>
{
    // Events to attach to:
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;
    
    public GameObject lid;

    private const string Title = 
        "Schritt 8: Gehäuseoberteil";
    private const string Description = 
        "Setzen Sie das Gehäuseoberteil auf das Gehäuseunterteil.";

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