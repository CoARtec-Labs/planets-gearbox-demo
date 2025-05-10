using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepGasket : BaseState
{
    private const String sceneName = "AssemblySteps";
    
    private const int PartClassID = 5;

    // Used to set scene loading on or off
    private bool keepSceneLoaded = false;

    public AssemblyStateStepGasket()
    {
        SceneName = "AssemblySteps";
    }
    
    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepGasket.OnNextClicked += NextClicked;
        AssemblyViewStepGasket.OnBackClicked += BackClicked;
        AssemblyViewStepGasket.OnStagingClicked += StagingClicked;
        
        AssemblyViewStepGasket.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepGasket.Instance.HideView();
        AssemblyViewStepGasket.OnNextClicked -= NextClicked;
        AssemblyViewStepGasket.OnBackClicked -= BackClicked;
        AssemblyViewStepGasket.OnStagingClicked -= StagingClicked;
        
        base.DestroyState();
    }

    private void NextClicked()
    {
        keepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepLid());
    }

    private void BackClicked()
    {
        keepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepCarrier());
    }
    
    private void StagingClicked()
    {
        keepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID));
    }

}
