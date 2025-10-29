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
public class DetectionState : BaseState
{
    // Label of the assembly object to be searched for
    private readonly int _searchObjectClassId;
    private readonly States _returningState;
    
    public DetectionState()
    {
        KeepSceneLoaded = false;
        SceneName = "Detection";
        _returningState = States.NONE;
        _searchObjectClassId = -1;
    }

    public DetectionState(int objectClassId) : this()
    {
        _searchObjectClassId = objectClassId;
    }
    
    public DetectionState(int objectClassId, States returningState) : this()
    {
        _searchObjectClassId = objectClassId;
        _returningState = returningState;
    }
    
    public override void PrepareState()
    {
        base.PrepareState();
        
        DetectionView.SearchObjectClassId = _searchObjectClassId;
        DetectionView.OnAssemblyClicked += AssemblyClicked;
        
        DetectionView.Instance.ShowView();
    }

    public override void DestroyState()
    {
        DetectionView.Instance.HideView();
        DetectionView.OnAssemblyClicked -= AssemblyClicked;
        DetectionView.SearchObjectClassId = -1;

        base.DestroyState();
    }
    
    private void AssemblyClicked()
    {
        Debug.Log("[StagingState.cs] assembly clicked.");

        KeepSceneLoaded = true;
        
        if (_returningState == States.NONE)
        {
            Owner.ChangeState(new AssemblyState00_Base());
        }
        else
        {
            Owner.ChangeState(StateFactory.CreateState(_returningState));
        }
    }
}
