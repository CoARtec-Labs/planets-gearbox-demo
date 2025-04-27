using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepLid : BaseState
{
    private const String sceneName = "AssemblySteps";

    // Used to set scene loading on or off
    private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();
        
        AssemblyViewStepLid.OnBackClicked += BackClicked;

        StateMachine.LoadScene(sceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        AssemblyViewStepLid.Instance.HideView();
        AssemblyViewStepLid.OnBackClicked -= BackClicked;

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
        Debug.Log($"Scene loaded: {scene.name}");
        SceneManager.SetActiveScene(scene);

        Debug.Log($"View instance exists: {AssemblyViewStepLid.Instance != null}");
        AssemblyViewStepLid.Instance.ShowView();
        AssemblyViewStepLid.Instance.ShowViewWithMesh();

        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    /// <summary>
    /// Function called when staging button was clicked.
    /// </summary>

    private void BackClicked()
    {
        keepSceneLoaded = true; 
        owner.ChangeState(new AssemblyStateStepGasket());
    }

}
