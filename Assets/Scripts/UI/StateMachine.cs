using System;
using System.Collections;
using System.Collections.Generic;
using UI.States;
using UI.Views.Assembly;
using UI.Views.Detection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// This state machine pattern is derived from this blog: 
// https://www.patrykgalach.com/2019/03/18/design-pattern-state-machine/

/// <summary>
/// State Machine keeps track of the currently active state and coordinates state transitions across scenes.
/// This class uses references to UIRoots in respective scenes for activation control.
/// These components are implemented as singletons in order to work across scene boundaries. 
/// </summary>
public class StateMachine : MonoBehaviour
{
    // UIRoot references:
    
    // [SerializeField]
    // private UIRootAssembly uiAssembly;
    // public UIRootAssembly UIAssembly => uiAssembly;
    //
    // [SerializeField]
    // private UIRootStaging uiStaging;
    // public UIRootStaging UIStaging => uiStaging;    
    
    [SerializeField, Tooltip("Scenes to be loaded at state-machine start.")]
    private List<string> scenesToLoad = new List<string>();
    [SerializeField, Tooltip("Initial state of the application.")]
    private States initialState = States.NONE;
    
    // Reference to currently operating state.
    private BaseState _currentState;
    
    /// <summary>
    /// Load scenes, activate initial scene and load initial state. This is the entry point to
    /// the StateMachine's states. Scene loading runs in background, everything else runs in callback after
    /// scene loading has completed.
    /// </summary>
    private void Start()
    {
        StartCoroutine(LoadMultiScenesAsync(scenesToLoad, StartInitialState));
        // StartCoroutine(LoadMultiScenesAsync("Assembly", "Detection", StartInitialState));
        // StartCoroutine(LoadSingleSceneAsync("Assembly", StartInitialState));
    }

    /// <summary>
    /// Calls the update function of the current state. This allows also states to perform frame updates.
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
    /// Public method to change state and to (un)load or (de)activate a scene. 
    /// </summary>
    /// <param name="newState">New state to be switched to. In case newState = null is passed, only
    /// the current state gets destroyed without creating and loading new one.</param>
    public void ChangeState(BaseState newState)
    {
        // Only do scene loading and activation if scenes are different
        var isSameScene = _currentState?.SceneName == newState?.SceneName;
        
        // If we currently have state, we need to destroy it.
        if (_currentState != null)
        {
            _currentState.DestroyState();
            
            if (!isSameScene)
            {
                // deactivate scene 
                DeactivateScene(_currentState.SceneName);
            
                // unload scene
                if (!_currentState.KeepSceneLoaded)
                {
                    SceneManager.UnloadSceneAsync(_currentState.SceneName);
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
                if (!IsSceneLoaded(_currentState.SceneName))
                {
                    SceneManager.sceneLoaded += SceneLoadedActivation;
                    SceneManager.LoadScene(_currentState.SceneName, LoadSceneMode.Additive);
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
        switch (sceneName)
        {
            case "Assembly":
                UIRootAssembly.Instance.ActivateSceneObjects();
                break;

            case "Detection":
                UIRootDetection.Instance.ActivateSceneObjects();
                break;

            case "Instructor":
                // nothing to do
                break;
            
            default:
                throw new System.ArgumentException("Unknown Scene");
        }
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
    
    private static void DeactivateScene(String sceneName)
    {
        switch (sceneName)
        {
            case "Assembly":
                UIRootAssembly.Instance.DeactivateSceneObjects();
                break;

            case "Detection":
                UIRootDetection.Instance.DeactivateSceneObjects();
                break;
            
            case "Instructor":
                // nothing to do
                break;

            default:
                throw new System.ArgumentException("Unknown Scene");
        }
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
    }

    private static bool IsSceneLoaded(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).IsValid();
    }
    
    private void SceneLoadedActivation(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= SceneLoadedActivation;
        
        ActivateScene(scene.name);
        
        _currentState.PrepareState();
        _currentState.Owner = this;
    }

    private void StartInitialState()
    {
        ChangeState(StateFactory.CreateState(States.Instructor00Base));
    }
    
    private delegate void LoadingScenesCompleteCallback();

    private static IEnumerator LoadMultiScenesAsync(List<string> sceneNames,
        LoadingScenesCompleteCallback loadingCompleteCallback)
    {
        if (sceneNames.Count == 0)
            yield break;
        
        foreach (var scene in sceneNames)
        {
            if (!IsSceneLoaded(scene))
            {
                // Do loading sequentially to avoid interference between game objects during loading.
                var asyncLoadScene = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

                while (!asyncLoadScene.isDone)
                {
                    yield return null;
                }
            }
            DeactivateScene(scene);
            
            Debug.Log(($"[StateMachine.cs] Loading scene {scene} done."));
        }
        loadingCompleteCallback?.Invoke();
    }
    
    private static IEnumerator LoadMultiScenesAsync(string sceneA, string sceneB, 
        LoadingScenesCompleteCallback callback)
    {
        
        if (!IsSceneLoaded(sceneA))
        {
            // Do loading sequentially to avoid interference between game objects during loading.
            var asyncLoadA = SceneManager.LoadSceneAsync(sceneA, LoadSceneMode.Additive);

            while (!asyncLoadA.isDone)
            {
                yield return null;
            }
        }

        DeactivateScene(sceneA);

        if (!IsSceneLoaded(sceneB))
        {
            var asyncLoadB = SceneManager.LoadSceneAsync(sceneB, LoadSceneMode.Additive);

            while (!asyncLoadB.isDone)
            {
                yield return null;
            }
        }

        DeactivateScene(sceneB);
        
        Debug.Log(($"[StateMachine.cs] Loading scenes {sceneA}, {sceneB} done."));

        callback();
    }
    
    private static IEnumerator LoadSingleSceneAsync (string scene, LoadingScenesCompleteCallback callback)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Debug.Log(($"[StateMachine.cs] Loading scene {scene} done."));

        callback();
    }

}
