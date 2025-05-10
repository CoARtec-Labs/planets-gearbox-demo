using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepBase : BaseState
{
    private const int PartClassID = 0;
    
    // Used to set scene loading on or off
    private bool _keepSceneLoaded = false;
    
    public AssemblyStateStepBase()
    {
        SceneName = "AssemblySteps";
    }

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepBase.OnStagingClicked += StagingClicked;
        AssemblyViewStepBase.OnNextClicked += NextClicked;
        AssemblyViewStepBase.Instance.ShowView(); 
    }

    public override void DestroyState()
    {
        AssemblyViewStepBase.Instance.HideView();
        AssemblyViewStepBase.OnStagingClicked -= StagingClicked;
        AssemblyViewStepBase.OnNextClicked -= NextClicked;
        
        base.DestroyState();
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void StagingClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Staging clicked.");

        _keepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID));
    }
    
    private void NextClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Next clicked.");

        _keepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepRing());
    }


}
