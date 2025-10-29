using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyState13_Planet2 : BaseState
{
    private const int PartClassID = 2; // planet-black
    
    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;
    
    public AssemblyState13_Planet2()
    {
        SceneName = "Assembly";
    }

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyView13_Planet2.OnNextClicked += NextClicked;
        AssemblyView13_Planet2.OnBackClicked += BackClicked;
        AssemblyView13_Planet2.OnStagingClicked += StagingClicked;
        AssemblyView13_Planet2.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyView13_Planet2.Instance.HideView();
        AssemblyView13_Planet2.OnNextClicked -= NextClicked;
        AssemblyView13_Planet2.OnBackClicked -= BackClicked;
        AssemblyView13_Planet2.OnStagingClicked -= StagingClicked;

        base.DestroyState();
    }

    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyState14_Planet3());

    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyState12_Planet1());
    }

    private void StagingClicked()
    {
        KeepSceneLoaded = false;
        Owner.ChangeState(new DetectionState(PartClassID, States.Assembly13Planet2));
    }
}
