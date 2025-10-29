using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyState17_Lid : BaseState
{
    // Used to set scene loading on or off
    // private bool _keepSceneLoaded = false;
    
    private const int PartClassID = 0; // lid-grey

    public AssemblyState17_Lid()
    {
        SceneName = "Assembly";
    }

    public override void PrepareState()
    {
        base.PrepareState();
        
        AssemblyView17_Lid.OnBackClicked += BackClicked;
        AssemblyView17_Lid.OnStagingClicked += StagingClicked;
        AssemblyView17_Lid.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyView17_Lid.Instance.HideView();
        AssemblyView17_Lid.OnBackClicked -= BackClicked;
        AssemblyView17_Lid.OnStagingClicked -= StagingClicked;

        base.DestroyState();
    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyState16_Gasket());
    }

    private void StagingClicked()
    {
        // Debug.Log("[AssemblyStateStepBase.cs] Staging clicked.");

        KeepSceneLoaded = false;
        Owner.ChangeState(new DetectionState(PartClassID, States.Assembly17Lid));
    }
}
