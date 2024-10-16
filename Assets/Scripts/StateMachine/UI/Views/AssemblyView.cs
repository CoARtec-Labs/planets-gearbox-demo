using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The AssemblyView singleton.
/// This classe is used to (de)activate the assembly UI.
/// </summary>
public class AssemblyView : BaseViewSingleton<AssemblyView>
{
    // Events to attach to.
    public static UnityAction OnStagingClicked;
    //public static UnityAction OnFinishClicked;

    /// <summary>
    /// Method for staging button.
    /// </summary>
    public void StagingClick()
    {
        OnStagingClicked?.Invoke();
    }


}
