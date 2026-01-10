using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Instructor
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState00_Base : BaseState
    {
        private const int PartClassID = -1; // all parts

        public AssemblyState00_Base()
        {
            SceneName = "Instructor";
        }

        public override void PrepareState()
        {
            base.PrepareState();

            AssemblyView00_Base.OnStagingClicked += StagingClicked;
            AssemblyView00_Base.OnNextClicked += NextClicked;
            AssemblyView00_Base.Instance.ShowView();
        }

        public override void DestroyState()
        {
            AssemblyView00_Base.Instance.HideView();
            AssemblyView00_Base.OnStagingClicked -= StagingClicked;
            AssemblyView00_Base.OnNextClicked -= NextClicked;

            base.DestroyState();
        }

        private void StagingClicked()
        {
            // Debug.Log("[AssemblyState00_Base.cs] Staging clicked.");

            KeepSceneLoaded = false;
            Owner.ChangeState(new Instructor.DetectionState05(PartClassID, States.Instructor00Base));
        }

        private void NextClicked()
        {
            // Debug.Log("[AssemblyState00_Base.cs] Next clicked.");

            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor10Ring));
        }


    }
}