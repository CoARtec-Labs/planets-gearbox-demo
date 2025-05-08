using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepWheelPlanet3 : BaseState
{
    // Used to set scene loading on or off
    private bool keepSceneLoaded = false;

    public AssemblyStateStepWheelPlanet3()
    {
        base.SceneName = "AssemblySteps";
    }
    
    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepWheelPlanet3.OnNextClicked += NextClicked;
        AssemblyViewStepWheelPlanet3.OnBackClicked += BackClicked;

        StateMachine.LoadScene(SceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        AssemblyViewStepWheelPlanet3.Instance.HideView();
        AssemblyViewStepWheelPlanet3.OnNextClicked -= NextClicked;
        AssemblyViewStepWheelPlanet3.OnBackClicked -= BackClicked;

        if (!keepSceneLoaded)
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
        Debug.Log($"Scene loaded: {scene.name}");
        SceneManager.SetActiveScene(scene);

        Debug.Log($"View instance exists: {AssemblyViewStepWheelPlanet3.Instance != null}");
        AssemblyViewStepWheelPlanet3.Instance.ShowView();
        AssemblyViewStepWheelPlanet3.Instance.ShowViewWithMesh();

        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>
    private void NextClicked()
    {
        keepSceneLoaded = true;
        Owner.ChangeState(new AssemblyStateStepCarrier());

    }

    private void BackClicked()
    {
        keepSceneLoaded = true; 
        Owner.ChangeState(new AssemblyStateStepWheelPlanet2());
    }

}
