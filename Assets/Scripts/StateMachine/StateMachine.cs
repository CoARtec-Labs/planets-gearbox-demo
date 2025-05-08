using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// This state machine pattern is derived from this blog: 
// https://www.patrykgalach.com/2019/03/18/design-pattern-state-machine/

/// <summary>
/// State Machine implementation.
/// Kepps track of the currently active state and coordinates state transitions.
/// </summary>
public class StateMachine : MonoBehaviour
{
    // Reference to currently operating state.
    private BaseState currentState;

    // Reference to UI root that hold references to all views in a scene.
    // These singletons can be used to 
    // This access is only available for views living in the same scene.
    // Views from other scenes must be accessed through the static interfaces
    // defined in each individual view.
    
    // [SerializeField]
    // private UIRootAssembly uiAssembly;
    // public UIRootAssembly UIAssembly => uiAssembly;
    //
    // [SerializeField]
    // private UIRootStaging uiStaging;
    // public UIRootStaging UIStaging => uiStaging;    
    
    public static int currentStepID=-1;

    /// <summary>
    /// Unity method called at start.
    /// This is the entry point to the StateMachine's states.
    /// </summary>
    private void Start()
    {
        DeactivateSceneForState("AssemblySteps");
        DeactivateSceneForState("PartsDetection");

        // Here we enter the state machine once play mode has started.
        // We start with the assembly instructions.
        // ChangeState(new AssemblyState());
        ChangeState(new AssemblyStateStepBase());
    }

    /// <summary>
    /// Unity method called each frame
    /// </summary>
    private void Update()
    {
        // If we have reference to state, we should update it!
        // Requires implementation inside the state only if needed.
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    /// <summary>
    /// Method used to change state and to (un)load or (de)activate a scene. 
    /// </summary>
    /// <param name="newState">New state to be switched to. In case newState = null is passed, only
    /// the current state gets destroyed without loading new one.</param>
    public void ChangeState(BaseState newState)
    {
        // If we currently have state, we need to destroy it!
        if (currentState != null)
        {
            currentState.DestroyState();
            
            // deactivate scene 
            DeactivateSceneForState(currentState.SceneName);
            
            // unload scene, if desired
        }

        // Swap reference
        currentState = newState;

        // If we decided to pass null as new state, nothing will happen.
        if (currentState != null)
        {
            currentState.PrepareState();
            currentState.Owner = this;

            // load corresponding scene if not yet loaded
            
            
            // activate corresponding scene
            ActivateSceneForState(currentState.SceneName);
        }
    }

    private void ActivateSceneForState(String sceneName)
    {
        if (sceneName == "AssemblySteps")
        {
            UIRootAssembly.Instance.ActivateSceneObjects();
        }
        else if (sceneName == "PartsDetection")
        {
            UIRootStaging.Instance.ActivateSceneObjects();
        }
        else
        {
            throw new System.ArgumentException("Unknown Scene");
        }
    }
    
    private void DeactivateSceneForState(String sceneName)
    {
        if (sceneName == "AssemblySteps")
        {
            UIRootAssembly.Instance.DeactivateSceneObjects();
        }
        else if (sceneName == "PartsDetection")
        {
            UIRootStaging.Instance.DeactivateSceneObjects();
        }
        else
        {
            throw new System.ArgumentException("Unknown Scene");
        }
    }

    /// <summary>
    /// Load with callback and activate the given scene depending on loading state.
    /// </summary>
    /// <param name="sceneName">The name of scene to be loaded.</param>
    /// <param name="callback">The function to be called after loading completed.</param>
    public static void LoadScene(String sceneName, UnityAction<Scene, LoadSceneMode> callback)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.IsValid())
        {
            Debug.Log($"[StateMachine.cs] Scene {sceneName} already loaded.");

            callback(scene, LoadSceneMode.Additive); // TODO: parameters not really needed here
        }
        else
        {
            Debug.Log($"[StateMachine.cs] Loading scene {sceneName}.");

            SceneManager.sceneLoaded += callback;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
    

}
