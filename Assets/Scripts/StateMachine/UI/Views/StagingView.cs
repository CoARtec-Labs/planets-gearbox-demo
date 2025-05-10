using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// The StagingView class.
/// This classe is used to (de)activate the staging UI.
/// </summary>
public class StagingView : BaseViewSingleton<StagingView>
{
    // Events to attach to.
    public static UnityAction OnAssemblyClicked;
    
    public static int SearchObjectClassId = -1;
    public int searchObjectClassId = -1;

    private new void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        //SearchObjectClassId = searchObjectClassId;
    }
    
    /// <summary>
    /// Method for assembly button.
    /// </summary>
    public void AssemblyClick()
    {
        OnAssemblyClicked?.Invoke();
    }
}
