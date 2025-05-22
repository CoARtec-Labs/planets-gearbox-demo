using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepWheelPlanet1 : BaseState
{
    private const int PartClassID = 1;
    
    public AssemblyStateStepWheelPlanet1()
    {
        SceneName = "AssemblySteps";
    }

    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepWheelPlanet1.OnNextClicked += NextClicked;
        AssemblyViewStepWheelPlanet1.OnBackClicked += BackClicked;
        AssemblyViewStepWheelPlanet1.OnStagingClicked += StagingClicked;
        AssemblyViewStepWheelPlanet1.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepWheelPlanet1.Instance.HideView();
        AssemblyViewStepWheelPlanet1.OnNextClicked -= NextClicked;
        AssemblyViewStepWheelPlanet1.OnBackClicked -= BackClicked;
        AssemblyViewStepWheelPlanet1.OnStagingClicked -= StagingClicked;

        base.DestroyState();
    }
    
    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepWheelPlanet2());

    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepWheelSun());
    }
    
    private void StagingClicked()
    {
        KeepSceneLoaded = false;
        Owner.ChangeState(new StagingState(PartClassID, States.AssemblyStepWheelPlanet1));
    }
    

}
