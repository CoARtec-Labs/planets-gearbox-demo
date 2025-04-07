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

    // public GameObject InstructionsCanvas;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    public void BackClick()
    {
        OnBackClicked?.Invoke();
    }

}