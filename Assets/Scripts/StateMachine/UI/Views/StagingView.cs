using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The StagingView class.
/// This classe is used to (de)activate the staging UI.
/// </summary>
public class StagingView : BaseViewSingleton<StagingView>
{
    // Events to attach to.
    public static UnityAction OnAssemblyClicked;

    /// <summary>
    /// Method for assembly button.
    /// </summary>
    public void AssemblyClick()
    {
        OnAssemblyClicked?.Invoke();
    }


}
