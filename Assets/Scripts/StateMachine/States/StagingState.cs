using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using statemachine;
using TMPro;

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
        KeepSceneLoaded = false;
        SceneName = "PartsDetection";
        _returningState = States.NONE;
        _searchObjectClassId = -1;
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
        
        StagingView.SearchObjectClassId = _searchObjectClassId;
        StagingView.OnAssemblyClicked += AssemblyClicked;
        
        StagingView.Instance.ShowView();
    }

    public override void DestroyState()
    {
        StagingView.Instance.HideView();
        StagingView.OnAssemblyClicked -= AssemblyClicked;
        StagingView.SearchObjectClassId = -1;

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
