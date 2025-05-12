using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepWheelSun : BaseState
{
    private const int PartClassID = 1;

    public AssemblyStateStepWheelSun()
    {
        SceneName = "AssemblySteps";
    }

    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepWheelSun.OnNextClicked += NextClicked;
        AssemblyViewStepWheelSun.OnBackClicked += BackClicked;
        AssemblyViewStepWheelSun.OnStagingClicked += StagingClicked;
        AssemblyViewStepWheelSun.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepWheelSun.Instance.HideView();
        AssemblyViewStepWheelSun.OnNextClicked -= NextClicked;
        AssemblyViewStepWheelSun.OnBackClicked -= BackClicked;
        AssemblyViewStepWheelSun.OnStagingClicked -= StagingClicked;
        
        base.DestroyState();
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepWheelPlanet1());

    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepRing());
    }

    private void StagingClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID, States.AssemblyStepWheelSun));
    }

}
