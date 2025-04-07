using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepWheelPlanet1 : BaseState
{
    private const String sceneName = "AssemblySteps";

    // Used to set scene loading on or off
    private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepWheelPlanet1.OnNextClicked += NextClicked;
        AssemblyViewStepWheelPlanet1.OnBackClicked += BackClicked;

        StateMachine.LoadScene(sceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        AssemblyViewStepWheelPlanet1.Instance.HideView();
        AssemblyViewStepWheelPlanet1.OnNextClicked -= NextClicked;
        AssemblyViewStepWheelPlanet1.OnBackClicked -= BackClicked;

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
        AssemblyViewStepWheelPlanet1.Instance.ShowView();

        // Clear this callback
        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void NextClicked()
    {
        keepSceneLoaded = true;
        owner.ChangeState(new AssemblyStateStepWheelPlanet2());

    }

    private void BackClicked()
    {
        keepSceneLoaded = true; 
        owner.ChangeState(new AssemblyStateStepWheelSun());
    }

}
