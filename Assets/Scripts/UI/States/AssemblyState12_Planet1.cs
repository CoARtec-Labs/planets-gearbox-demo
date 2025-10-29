using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyState12_Planet1 : BaseState
{
    private const int PartClassID = 1; // planet-white
    
    public AssemblyState12_Planet1()
    {
        SceneName = "Assembly";
    }

    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyView12_Planet1.OnNextClicked += NextClicked;
        AssemblyView12_Planet1.OnBackClicked += BackClicked;
        AssemblyView12_Planet1.OnStagingClicked += StagingClicked;
        AssemblyView12_Planet1.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyView12_Planet1.Instance.HideView();
        AssemblyView12_Planet1.OnNextClicked -= NextClicked;
        AssemblyView12_Planet1.OnBackClicked -= BackClicked;
        AssemblyView12_Planet1.OnStagingClicked -= StagingClicked;

        base.DestroyState();
    }
    
    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyState13_Planet2());

    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyState11_Sun());
    }
    
    private void StagingClicked()
    {
        KeepSceneLoaded = false;
        Owner.ChangeState(new DetectionState(PartClassID, States.Assembly12Planet1));
    }
    

}
