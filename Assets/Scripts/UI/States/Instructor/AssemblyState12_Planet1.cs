using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Instructor
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState12_Planet1 : BaseState
    {
        private const int PartClassID = 1; // planet-white

        public AssemblyState12_Planet1()
        {
            SceneName = "Instructor";
        }

        // Used to set scene loading on or off
        // private bool keepSceneLoaded = false;

        public override void PrepareState()
        {
            base.PrepareState();

            AssemblyView12_Planet1.OnNextClicked += NextClicked;
            AssemblyView12_Planet1.OnBackClicked += BackClicked;
            AssemblyView12_Planet1.OnStagingClicked += StagingClicked;
            AssemblyView12_Planet1.Instance.ShowView();
        }

        public override void DestroyState()
        {
            AssemblyView12_Planet1.Instance.HideView();
            AssemblyView12_Planet1.OnNextClicked -= NextClicked;
            AssemblyView12_Planet1.OnBackClicked -= BackClicked;
            AssemblyView12_Planet1.OnStagingClicked -= StagingClicked;

            base.DestroyState();
        }

        private void NextClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor13Planet2));
        }

        private void BackClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor11Sun));
        }

        private void StagingClicked()
        {
            KeepSceneLoaded = false;
            Owner.ChangeState(new Instructor.DetectionState05(PartClassID, States.Instructor12Planet1));
        }
    }
}