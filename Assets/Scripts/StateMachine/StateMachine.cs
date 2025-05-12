using System;
using System.Collections;

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

    // Reference to currently operating state.
    private BaseState _currentState;
    
    /// <summary>
    /// Load all scenes, activate initial scene and load initial state. This is the entry point to
    /// the StateMachine's states. Scene loading runs in background, everything else runs in callback after
    /// scene loading has completed.
    /// </summary>
    private void Start()
    {
        StartCoroutine(LoadScenesBlocking("AssemblySteps", "PartsDetection", StartInitialState));
    }

    /// <summary>
    /// This allows states to perform frame updates.
    /// </summary>
    private void Update()
    {
        // If we have reference to state, we should update it!
        // Requires implementation inside the state only if needed.
        if (_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    /// <summary>
    /// Method used to change state and to (un)load or (de)activate a scene. 
    /// </summary>
    /// <param name="newState">New state to be switched to. In case newState = null is passed, only
    /// the current state gets destroyed without creating and loading new one.</param>
    public void ChangeState(BaseState newState)
    {
        // Only do scene loading and activation if scenes are different
        bool isSameScene = _currentState?.SceneName == newState?.SceneName;
        
        // If we currently have state, we need to destroy it!
        if (_currentState != null)
        {
            _currentState.DestroyState();
            
            if (!isSameScene) // && !_currentState.KeepSceneLoaded)
            {
                // deactivate scene 
                DeactivateScene(_currentState.SceneName);
            
                // unload scene
                if (_currentState.SceneName == "AssemblySteps")
                {
                    SceneManager.UnloadSceneAsync("AssemblySteps");
                }
            }
        }

        // Swap reference
        _currentState = newState;

        // If we decided to pass null as new state, nothing will happen.
        if (_currentState != null)
        {
            if (!isSameScene)
            {
                // load corresponding scene
                if (_currentState.SceneName == "AssemblySteps")
                {
                    SceneManager.sceneLoaded += SceneLoadedNoUIRootActivation;
                    SceneManager.LoadScene("AssemblySteps", LoadSceneMode.Additive);
                    return;
                }
                
                // activate corresponding scene
                ActivateScene(_currentState.SceneName);
            }
            
            _currentState.PrepareState();
            _currentState.Owner = this;
        }
    }

    private void ActivateScene(String sceneName)
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
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
    
    private void DeactivateScene(String sceneName)
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
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
    }

    private void SceneLoadedNoUIRootActivation(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= SceneLoadedNoUIRootActivation;
        SceneManager.SetActiveScene((Scene)SceneManager.GetSceneByName(scene.name));
        
        _currentState.PrepareState();
        _currentState.Owner = this;
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

    private void StartInitialState()
    {
        // ActivateScene("AssemblySteps");
        ChangeState(new AssemblyStateStepBase());
    }
    
    private delegate void LoadingScenesCompleteCallback();
    
    private static IEnumerator LoadScenesBlocking (string sceneA, string sceneB, 
        LoadingScenesCompleteCallback callback)
    {
        // Do loading sequentially to avoid interference between game objects during loading.
        // var asyncLoadA = SceneManager.LoadSceneAsync(sceneA, LoadSceneMode.Additive);
        //
        // while (!asyncLoadA.isDone)
        // {
        //     yield return null;
        // }
        
        var asyncLoadB = SceneManager.LoadSceneAsync(sceneB, LoadSceneMode.Additive);

        while (!asyncLoadB.isDone)
        {
            yield return null;
        }
        
        Debug.Log(($"Loading {sceneA}, {sceneB} done."));

        callback();
    }

}
