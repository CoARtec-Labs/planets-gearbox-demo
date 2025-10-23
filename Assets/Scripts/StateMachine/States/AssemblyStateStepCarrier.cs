using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepCarrier : BaseState
{
    private const int PartClassID = 6; // carrier-black 
    
    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;
    
    public AssemblyStateStepCarrier()
    {
        SceneName = "AssemblySteps";
    }
    
    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepCarrier.OnNextClicked += NextClicked;
        AssemblyViewStepCarrier.OnBackClicked += BackClicked;
        AssemblyViewStepCarrier.OnStagingClicked += StagingClicked;
        AssemblyViewStepCarrier.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepCarrier.Instance.HideView();
        AssemblyViewStepCarrier.OnNextClicked -= NextClicked;
        AssemblyViewStepCarrier.OnBackClicked -= BackClicked;
        AssemblyViewStepCarrier.OnStagingClicked -= StagingClicked;
        
        base.DestroyState();
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepGasket());

    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepWheelPlanet3());
    }

    private void StagingClicked()
    {
        KeepSceneLoaded = false;
        Owner.ChangeState(new StagingState(PartClassID, States.AssemblyStepCarrier));
    }
}
