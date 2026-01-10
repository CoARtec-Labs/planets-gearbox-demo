using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Instructor
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState10_Ring : BaseState
    {
        // Used to set scene loading on or off
        // private bool keepSceneLoaded = false;
        private const int PartClassID = 8; // ring-gray

        public AssemblyState10_Ring()
        {
            SceneName = "Instructor";
        }

        public override void PrepareState()
        {
            base.PrepareState();

            AssemblyView10_Ring.OnNextClicked += NextClicked;
            AssemblyView10_Ring.OnBackClicked += BackClicked;
            AssemblyView10_Ring.OnStagingClicked += StagingClicked;
            AssemblyView10_Ring.Instance.ShowView();
        }

        public override void DestroyState()
        {
            AssemblyView10_Ring.Instance.HideView();
            AssemblyView10_Ring.OnNextClicked -= NextClicked;
            AssemblyView10_Ring.OnBackClicked -= BackClicked;
            AssemblyView10_Ring.OnStagingClicked -= StagingClicked;

            base.DestroyState();
        }

        private void NextClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor11Sun));
        }

        private void BackClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor00Base));
        }

        private void StagingClicked()
        {
            KeepSceneLoaded = false;
            
            Owner.ChangeState(new Instructor.DetectionState05(PartClassID, States.Instructor10Ring));
        }

    }
}