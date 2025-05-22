using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using statemachine;

/// <summary>
/// Staging state: detection and highlighting of assembly parts.
/// </summary>
public class StagingState : BaseState
{
    // Label of the assembly object to be searched for
    private readonly int _searchObjectClassId;
    private readonly States _returningState;

    public StagingState()
    {
        SceneName = "PartsDetection";
        _returningState = States.NONE;
    }

    public StagingState(int objectClassId) : this()
    {
        _searchObjectClassId = objectClassId;
    }
    
    public StagingState(int objectClassId, States returningState) : this()
    {
        _searchObjectClassId = objectClassId;
        _returningState = returningState;
    }
    
    public override void PrepareState()
    {
        base.PrepareState();
        
        StagingView.OnAssemblyClicked += AssemblyClicked;
        StagingView.Instance.ShowView();
        
        StagingView.SearchObjectClassId = _searchObjectClassId;
    }

    public override void DestroyState()
    {
        StagingView.Instance.HideView();
        StagingView.OnAssemblyClicked -= AssemblyClicked;

        base.DestroyState();
    }
    
    private void AssemblyClicked()
    {
        Debug.Log("[StagingState.cs] assembly clicked.");

        KeepSceneLoaded = true;
        
        if (_returningState == States.NONE)
        {
            Owner.ChangeState(new AssemblyStateStepBase());
        }
        else
        {
            Owner.ChangeState(StateFactory.CreateState(_returningState));
        }
    }
}
