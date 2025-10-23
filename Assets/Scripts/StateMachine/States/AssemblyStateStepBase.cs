using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepBase : BaseState
{
    private const int PartClassID = -1; // all parts
    
    // Used to set scene loading on or off
    // public bool KeepSceneLoaded = false;
    
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
    
    private void StagingClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Staging clicked.");

        KeepSceneLoaded = false;
        Owner.ChangeState(new StagingState(PartClassID, States.AssemblyStepBase));
    }
    
    private void NextClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Next clicked.");

        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepRing());
    }


}
