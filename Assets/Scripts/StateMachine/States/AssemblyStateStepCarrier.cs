using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepCarrier : BaseState
{
    private const int PartClassID = 0;
    
    // Used to set scene loading on or off
    private bool keepSceneLoaded = false;
    
    public AssemblyStateStepCarrier()
    {
        SceneName = "AssemblySteps";
    }
    
    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepCarrier.OnNextClicked += NextClicked;
        AssemblyViewStepCarrier.OnBackClicked += BackClicked;
        AssemblyViewStepCarrier.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepCarrier.Instance.HideView();
        AssemblyViewStepCarrier.OnNextClicked -= NextClicked;
        AssemblyViewStepCarrier.OnBackClicked -= BackClicked;
        
        base.DestroyState();
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void NextClicked()
    {
        keepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepGasket());

    }

    private void BackClicked()
    {
        keepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepWheelPlanet3());
    }

    private void StagingClicked()
    {
        keepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID));
    }
}
