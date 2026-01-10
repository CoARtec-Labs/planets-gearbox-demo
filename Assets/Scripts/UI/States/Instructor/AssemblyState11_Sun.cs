using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Instructor
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState11_Sun : BaseState
    {
        private const int PartClassID = 9; // sun-gold

        public AssemblyState11_Sun()
        {
            SceneName = "Instructor";
        }

        // Used to set scene loading on or off
        // private bool keepSceneLoaded = false;

        public override void PrepareState()
        {
            base.PrepareState();

            AssemblyView11_Sun.OnNextClicked += NextClicked;
            AssemblyView11_Sun.OnBackClicked += BackClicked;
            AssemblyView11_Sun.OnStagingClicked += StagingClicked;
            AssemblyView11_Sun.Instance.ShowView();
        }

        public override void DestroyState()
        {
            AssemblyView11_Sun.Instance.HideView();
            AssemblyView11_Sun.OnNextClicked -= NextClicked;
            AssemblyView11_Sun.OnBackClicked -= BackClicked;
            AssemblyView11_Sun.OnStagingClicked -= StagingClicked;

            base.DestroyState();
        }

        /// <summary>
        /// Function called when staging button was clicked.
        /// </summary>
        private void NextClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor12Planet1));
        }

        private void BackClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor10Ring));
        }

        private void StagingClicked()
        {
            KeepSceneLoaded = false;
            Owner.ChangeState(new Instructor.DetectionState05(PartClassID, States.Instructor11Sun));
        }

    }
}