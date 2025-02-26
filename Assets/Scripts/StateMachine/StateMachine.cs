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

    // Reference to UI root that holds references to different views.
    // THis access is only available for views living in the same scene.
    // Views from other scenes must be accessed thorugh the static interfaces
    // defined in each individual view.
    
    [SerializeField]
    private UIRootAssembly uiAssembly;
    public UIRootAssembly UIAssembly => uiAssembly;

    [SerializeField]
    private UIRootStaging uiStaging;
    public UIRootStaging UIStaging => uiStaging;    
    
    public static int currentStepID=-1;

    /// <summary>
    /// Unity method called at start.
    /// This is the entry point to the StateMachine's states.
    /// </summary>
    private void Start()
    {
        // SceneManager.LoadScene(StagingState.sceneName, LoadSceneMode.Additive);

        // Start with the assembly instructions
        ChangeState(new AssemblyState());
    }

    /// <summary>
    /// Unity method called each frame
    /// </summary>
    private void Update()
    {
        // If we have reference to state, we should update it!
        // Requirees implementation inside the state only if needed.
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    /// <summary>
    /// Method used to change state
    /// </summary>
    /// <param name="newState">New state</param>
    public void ChangeState(BaseState newState)
    {
        // If we currently have state, we need to destroy it!
        if (currentState != null)
        {
            currentState.DestroyState();
        }

        // Swap reference
        currentState = newState;

        // If we passed reference to new state, we should assign owner of that state and initialize it!
        // If we decided to pass null as new state, nothing will happened.
        if (currentState != null)
        {
            currentState.owner = this;
            currentState.PrepareState();
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

