using System;
using statemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyState16_Gasket : BaseState
{
    private const String sceneName = "Assembly";
    
    private const int PartClassID = 7; // gasket-gold 

    // Used to set scene loading on or off
    // private bool keepSceneLoaded = false;

    public AssemblyState16_Gasket()
    {
        SceneName = "Assembly";
    }
    
    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyView16_Gasket.OnNextClicked += NextClicked;
        AssemblyView16_Gasket.OnBackClicked += BackClicked;
        AssemblyView16_Gasket.OnStagingClicked += StagingClicked;
        
        AssemblyView16_Gasket.Instance.ShowView();
    }

    public override void DestroyState()
    {
        AssemblyView16_Gasket.Instance.HideView();
        AssemblyView16_Gasket.OnNextClicked -= NextClicked;
        AssemblyView16_Gasket.OnBackClicked -= BackClicked;
        AssemblyView16_Gasket.OnStagingClicked -= StagingClicked;
        
        base.DestroyState();
    }

    private void NextClicked()
    {
        KeepSceneLoaded = true;
        Owner.ChangeState(new AssemblyState17_Lid());
    }

    private void BackClicked()
    {
        KeepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyState15_Carrier());
    }
    
    private void StagingClicked()
    {
        KeepSceneLoaded = false;
        Owner.ChangeState(new DetectionState(PartClassID, States.Assembly16Gasket));
    }

}
