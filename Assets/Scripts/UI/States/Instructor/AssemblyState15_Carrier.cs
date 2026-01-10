using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Instructor
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState15_Carrier : BaseState
    {
        private const int PartClassID = 6; // carrier-black 

        // Used to set scene loading on or off
        // private bool keepSceneLoaded = false;

        public AssemblyState15_Carrier()
        {
            SceneName = "Instructor";
        }

        public override void PrepareState()
        {
            base.PrepareState();

            AssemblyView15_Carrier.OnNextClicked += NextClicked;
            AssemblyView15_Carrier.OnBackClicked += BackClicked;
            AssemblyView15_Carrier.OnStagingClicked += StagingClicked;
            AssemblyView15_Carrier.Instance.ShowView();
        }

        public override void DestroyState()
        {
            AssemblyView15_Carrier.Instance.HideView();
            AssemblyView15_Carrier.OnNextClicked -= NextClicked;
            AssemblyView15_Carrier.OnBackClicked -= BackClicked;
            AssemblyView15_Carrier.OnStagingClicked -= StagingClicked;

            base.DestroyState();
        }

        /// <summary>
        /// Function called when staging button was clicked.
        /// </summary>
        private void NextClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor16Gasket));

        }

        private void BackClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor14Planet3));
        }

        private void StagingClicked()
        {
            KeepSceneLoaded = false;
            Owner.ChangeState(new Instructor.DetectionState05(PartClassID, States.Instructor15Carrier));
        }
    }
}