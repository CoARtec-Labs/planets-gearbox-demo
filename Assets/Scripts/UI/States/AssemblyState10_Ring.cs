using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyState10_Ring : BaseState
{
    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;
    private const int PartClassID = 8; // ring-gray
    
    public AssemblyState10_Ring()
    {
        SceneName = "Assembly";
    }

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyView10_Ring.OnNextClicked += NextClicked;
        AssemblyView10_Ring.OnBackClicked += BackClicked;
        AssemblyView10_Ring.OnStagingClicked += StagingClicked;
        AssemblyView10_Ring.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyView10_Ring.Instance.HideView();
        AssemblyView10_Ring.OnNextClicked -= NextClicked;
        AssemblyView10_Ring.OnBackClicked -= BackClicked;
        AssemblyView10_Ring.OnStagingClicked -= StagingClicked;
        
        base.DestroyState();
    }
    
    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyState11_Sun());
    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyState00_Base());
    }
    
    private void StagingClicked()
    {
        KeepSceneLoaded = false;
        Owner.ChangeState(new DetectionState(PartClassID, States.Assembly10Ring));
    }

}
