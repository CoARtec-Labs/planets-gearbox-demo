using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepWheelPlanet3 : BaseState
{
    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;
    
    private const int PartClassID = 2;
    
    public AssemblyStateStepWheelPlanet3()
    {
        SceneName = "AssemblySteps";
    }
    
    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepWheelPlanet3.OnNextClicked += NextClicked;
        AssemblyViewStepWheelPlanet3.OnBackClicked += BackClicked;
        AssemblyViewStepWheelPlanet3.OnStagingClicked += StagingClicked;
        AssemblyViewStepWheelPlanet3.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepWheelPlanet3.Instance.HideView();
        AssemblyViewStepWheelPlanet3.OnNextClicked -= NextClicked;
        AssemblyViewStepWheelPlanet3.OnBackClicked -= BackClicked;
        AssemblyViewStepWheelPlanet3.OnStagingClicked -= StagingClicked;
        
        base.DestroyState();
    }
    
    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepCarrier());
    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepWheelPlanet2());
    }

    private void StagingClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID, States.AssemblyStepWheelPlanet3));
    }
}
