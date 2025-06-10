using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepWheelSun : BaseViewSingleton<AssemblyViewStepWheelSun>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject sun;
    
    private const string Title = "Sun";
    private const string Description = 
        "Platzieren Sie das Zahnrad 'Sun' in der Mitte des Rings.";

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