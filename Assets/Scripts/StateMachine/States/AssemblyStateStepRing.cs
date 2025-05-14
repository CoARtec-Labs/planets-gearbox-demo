using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepRing : BaseState
{
    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;
    private const int PartClassID = -1;
    
    public AssemblyStateStepRing()
    {
        SceneName = "AssemblySteps";
    }

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepRing.OnNextClicked += NextClicked;
        AssemblyViewStepRing.OnBackClicked += BackClicked;
        AssemblyViewStepRing.OnStagingClicked += StagingClicked;
        AssemblyViewStepRing.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepRing.Instance.HideView();
        AssemblyViewStepRing.OnNextClicked -= NextClicked;
        AssemblyViewStepRing.OnBackClicked -= BackClicked;
        AssemblyViewStepRing.OnStagingClicked -= StagingClicked;
        
        base.DestroyState();
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepWheelSun());
    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepBase());
    }
    
    private void StagingClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID, States.AssemblyStepRing));
    }

}
