using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyState14_Planet3 : BaseState
{
    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;
    
    private const int PartClassID = 5; // planet-green
    
    public AssemblyState14_Planet3()
    {
        SceneName = "Assembly";
    }
    
    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyView14_Planet3.OnNextClicked += NextClicked;
        AssemblyView14_Planet3.OnBackClicked += BackClicked;
        AssemblyView14_Planet3.OnStagingClicked += StagingClicked;
        AssemblyView14_Planet3.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyView14_Planet3.Instance.HideView();
        AssemblyView14_Planet3.OnNextClicked -= NextClicked;
        AssemblyView14_Planet3.OnBackClicked -= BackClicked;
        AssemblyView14_Planet3.OnStagingClicked -= StagingClicked;
        
        base.DestroyState();
    }
    
    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyState15_Carrier());
    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyState13_Planet2());
    }

    private void StagingClicked()
    {
        KeepSceneLoaded = false;
        Owner.ChangeState(new DetectionState(PartClassID, States.Assembly14Planet3));
    }
}
