using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyState : BaseState
{
    private const String sceneName = "AssemblySteps";

    // Variables used for loading and destroying game content
    public bool loadGameContent = true;
    public bool destroyGameContent = true;

    private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyView.OnStagingClicked += StagingClicked;
        AssemblyView.OnStepBaseClicked += StepBaseClicked;

        //StateMachine.LoadScene(sceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        AssemblyView.Instance.HideView();
        
        AssemblyView.OnStagingClicked -= StagingClicked;
        AssemblyView.OnStepBaseClicked -= StepBaseClicked;

        if (!keepSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            
            // Shift lighting and editing defaults to main scene.
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
        }

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
        Debug.Log($"[AssemblyState.cs] Activating scene {scene.name}.");

        SceneManager.SetActiveScene(scene);

        // Show menu view
        AssemblyView.Instance.ShowView();

        // Clear this callback
        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void StagingClicked()
    {
        Debug.Log("[AssemblyState.cs] staging clicked.");

        keepSceneLoaded = false;
        // owner.ChangeState(new StagingState());
    }
    
    private void StepBaseClicked()
    {
        Debug.Log("[AssemblyState.cs] step base clicked.");

        keepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepBase());
    }


}
