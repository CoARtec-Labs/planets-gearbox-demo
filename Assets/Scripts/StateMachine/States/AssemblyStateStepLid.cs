using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepLid : BaseState
{
    // Used to set scene loading on or off
    // private bool _keepSceneLoaded = false;
    
    private const int PartClassID = 6;

    public AssemblyStateStepLid()
    {
        SceneName = "AssemblySteps";
    }

    public override void PrepareState()
    {
        base.PrepareState();
        
        AssemblyViewStepLid.OnBackClicked += BackClicked;
        AssemblyViewStepLid.OnStagingClicked += StagingClicked;
        AssemblyViewStepLid.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepLid.Instance.HideView();
        AssemblyViewStepLid.OnBackClicked -= BackClicked;
        AssemblyViewStepLid.OnStagingClicked -= StagingClicked;

        base.DestroyState();
    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepGasket());
    }

    private void StagingClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Staging clicked.");

        KeepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID, States.AssemblyStepLid));
    }
}
