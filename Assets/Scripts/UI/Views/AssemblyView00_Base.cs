using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyView00_Base : BaseViewSingleton<AssemblyView00_Base>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnStagingClicked;
    public static UnityAction OnNextClicked;

    // public GameObject InstructionsCanvas;

    private void Awake()
    {
        base.Awake();
        
        Debug.Log("[AssemblyViewStepBase.cs] Awake");
    }
    
    /// <summary>
    /// Method for staging button.
    /// </summary>
    public void AssemblyClick()
    {
        OnStagingClicked?.Invoke();
    }

    public void NextClicked()
    {
        OnNextClicked?.Invoke();
    }


}
