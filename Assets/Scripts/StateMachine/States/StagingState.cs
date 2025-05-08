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
    private int _searchObjectId = -1;

    public StagingState(int objectId)
    {
        base.SceneName = "PartsDetection";
        this._searchObjectId = objectId;
    }
    
    public override void PrepareState()
    {
        base.PrepareState();

        // Attach functions to view events.
        StagingView.OnAssemblyClicked += AssemblyClicked;

        // Do everything else after scene is loaded.
        StateMachine.LoadScene(SceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        // Hide menu view
        StagingView.Instance.HideView();

        // Detach functions from view events
        StagingView.OnAssemblyClicked -= AssemblyClicked;

        // SceneManager.UnloadSceneAsync(sceneName);

        // Turn off light and editing for this scene.
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

        base.DestroyState();
    }

    /// <summary>
    /// Callback for object initialization after scene is loaded.
    /// This is required, since scene loading is happening in the backgound 
    /// over multiple frame updates. Parameters are handled internally.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[StagingState.cs] Scene {scene.name} is loaded, activating.");

        SceneManager.SetActiveScene(scene);

        // Show menu view
        StagingView.Instance.ShowView();
        
        // Hand over object label to view
        StagingView.Instance.searchObjectClassId= _searchObjectId;

        // Clear this callback
        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    /// <summary>
    /// Function called when assembly button is clicked.
    /// </summary>
    private void AssemblyClicked()
    {
        Debug.Log("[StagingState.cs] assembly step base clicked.");

        Owner.ChangeState(new AssemblyStateStepBase());
    }

}
