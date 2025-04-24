using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the assembly state.
/// </summary>
public class AssemblyStateStepRing : BaseState
{
    private const String sceneName = "AssemblySteps";
    private bool keepSceneLoaded = false;

    public override void PrepareState()
    {
        base.PrepareState();

        AssemblyViewStepRing.OnNextClicked += NextClicked;
        AssemblyViewStepRing.OnBackClicked += BackClicked;

        StateMachine.LoadScene(sceneName, SceneLoadedCallback);
    }

    public override void DestroyState()
    {
        AssemblyViewStepRing.Instance.HideView();
        AssemblyViewStepRing.OnNextClicked -= NextClicked;
        AssemblyViewStepRing.OnBackClicked -= BackClicked;

        if (!keepSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
        }

        base.DestroyState();
    }

    private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded Callback Triggered");
        SceneManager.SetActiveScene(scene);

        AssemblyViewStepRing.Instance.ShowView();
        SceneManager.sceneLoaded -= SceneLoadedCallback;
    }

    private void NextClicked()
    {
        keepSceneLoaded = true;
        owner.ChangeState(new AssemblyStateStepWheelSun());
    }

    private void BackClicked()
    {
        keepSceneLoaded = true;
        owner.ChangeState(new AssemblyStateStepBase());
    }
}