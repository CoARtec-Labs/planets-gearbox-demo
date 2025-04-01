using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepBase : BaseViewSingleton<AssemblyViewStepBase>
{
    // Events to attach to.
    // public static UnityAction OnStagingClicked;
    public static UnityAction OnAssemblyClicked;
    public static UnityAction OnNextClicked;

    // public GameObject InstructionsCanvas;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    public void AssemblyClick()
    {
        OnAssemblyClicked?.Invoke();
    }

    public void NextClicked()
    {
        OnNextClicked?.Invoke();
    }


}
