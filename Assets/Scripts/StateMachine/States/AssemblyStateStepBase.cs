using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepBase : BaseState
{
    private const int PartClassID = 0;

    public AssemblyStateStepBase()
    {
        base.SceneName = "AssemblySteps";
    }

    // Used to set scene loading on or off
    private bool _keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepBase.OnAssemblyClicked += StagingClicked;
        AssemblyViewStepBase.OnNextClicked += NextClicked;

        StateMachine.LoadScene(SceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        AssemblyViewStepBase.Instance.HideView();
        AssemblyViewStepBase.OnAssemblyClicked -= StagingClicked;
        AssemblyViewStepBase.OnNextClicked -= NextClicked;

        if (!_keepSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(SceneName);
            
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
        Debug.Log($"[AssemblyStateStepBase.cs] Activating scene {scene.name}.");

        SceneManager.SetActiveScene(scene);

        // Show menu view
        AssemblyViewStepBase.Instance.ShowView();

        // Clear this callback
        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void StagingClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Staging clicked.");

        _keepSceneLoaded = true;
        Owner.ChangeState(new StagingState(PartClassID));
    }
    
    private void NextClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Next clicked.");

        _keepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepRing());
    }


}
