using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepGasket : BaseState
{
    private const String sceneName = "AssemblySteps";

    // Used to set scene loading on or off
    private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepGasket.OnNextClicked += NextClicked;
        AssemblyViewStepGasket.OnBackClicked += BackClicked;

        StateMachine.LoadScene(sceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        AssemblyViewStepGasket.Instance.HideView();
        AssemblyViewStepGasket.OnNextClicked -= NextClicked;
        AssemblyViewStepGasket.OnBackClicked -= BackClicked;

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
        SceneManager.SetActiveScene(scene);

        // Show menu view
        AssemblyViewStepGasket.Instance.ShowView();

        // Clear this callback
        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void NextClicked()
    {
        keepSceneLoaded = true;
        owner.ChangeState(new AssemblyStateStepLid());

    }

    private void BackClicked()
    {
        keepSceneLoaded = true; 
        owner.ChangeState(new AssemblyStateStepCarrier());
    }

}
