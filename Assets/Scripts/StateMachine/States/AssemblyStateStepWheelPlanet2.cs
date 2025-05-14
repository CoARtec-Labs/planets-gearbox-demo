using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepWheelPlanet2 : BaseState
{
    private const int PartClassID = 2;
    
    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;
    
    public AssemblyStateStepWheelPlanet2()
    {
        SceneName = "AssemblySteps";
    }

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepWheelPlanet2.OnNextClicked += NextClicked;
        AssemblyViewStepWheelPlanet2.OnBackClicked += BackClicked;
        AssemblyViewStepWheelPlanet2.OnStagingClicked += StagingClicked;
        AssemblyViewStepWheelPlanet2.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepWheelPlanet2.Instance.HideView();
        AssemblyViewStepWheelPlanet2.OnNextClicked -= NextClicked;
        AssemblyViewStepWheelPlanet2.OnBackClicked -= BackClicked;
        AssemblyViewStepWheelPlanet2.OnStagingClicked -= StagingClicked;

        base.DestroyState();
    }

    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepWheelPlanet3());

    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepWheelPlanet1());
    }

    private void StagingClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID, States.AssemblyStepWheelPlanet2));
    }
}
