using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Staging state: detection and highlighting of assembly parts.
/// </summary>
public class StagingState : BaseState
{
    // Label of the assembly object to be searched for
    private readonly int _searchObjectClassId;

    public StagingState(int objectClassId)
    {
        SceneName = "PartsDetection";
        
        _searchObjectClassId = objectClassId;
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
        Debug.Log("[StagingState.cs] assembly step base clicked.");

        Owner.ChangeState(new AssemblyStateStepBase());
    }

}
