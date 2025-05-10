using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepLid : BaseState
{
    // Used to set scene loading on or off
    private bool keepSceneLoaded = false;
    
    public AssemblyStateStepLid()
    {
        SceneName = "AssemblySteps";
    }

    public override void PrepareState()
    {
        base.PrepareState();
        
        AssemblyViewStepLid.OnBackClicked += BackClicked;
        AssemblyViewStepLid.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyViewStepLid.Instance.HideView();
        AssemblyViewStepLid.OnBackClicked -= BackClicked;

        base.DestroyState();
    }

    private void BackClicked()
    {
        keepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepGasket());
    }

}
