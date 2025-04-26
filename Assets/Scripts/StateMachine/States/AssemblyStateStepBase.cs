using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepBase : BaseState
{
    private const String sceneName = "AssemblySteps";

    // Used to set scene loading on or off
    private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepBase.OnAssemblyClicked += AssemblyClicked;
        AssemblyViewStepBase.OnNextClicked += NextClicked;

        StateMachine.LoadScene(sceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        AssemblyViewStepBase.Instance.HideView();
        AssemblyViewStepBase.OnAssemblyClicked -= AssemblyClicked;
        AssemblyViewStepBase.OnNextClicked -= NextClicked;

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
    private void AssemblyClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Staging clicked.");

        keepSceneLoaded = true;
        owner.ChangeState(new StagingState());
    }
    
    private void NextClicked()
    {
        Debug.Log("[AssemblyStateStepBase.cs] Next clicked.");

        keepSceneLoaded = true;
        owner.ChangeState(new AssemblyStateStepRing());
    }


}
