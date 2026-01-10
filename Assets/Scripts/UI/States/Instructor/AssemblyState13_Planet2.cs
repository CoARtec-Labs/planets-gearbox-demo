using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Instructor
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState13_Planet2 : BaseState
    {
        private const int PartClassID = 2; // planet-black

        public AssemblyState13_Planet2()
        {
            SceneName = "Instructor";
        }

        public override void PrepareState()
        {
            base.PrepareState();

            AssemblyView13_Planet2.OnNextClicked += NextClicked;
            AssemblyView13_Planet2.OnBackClicked += BackClicked;
            AssemblyView13_Planet2.OnStagingClicked += StagingClicked;
            AssemblyView13_Planet2.Instance.ShowView();
        }

        public override void DestroyState()
        {
            AssemblyView13_Planet2.Instance.HideView();
            AssemblyView13_Planet2.OnNextClicked -= NextClicked;
            AssemblyView13_Planet2.OnBackClicked -= BackClicked;
            AssemblyView13_Planet2.OnStagingClicked -= StagingClicked;

            base.DestroyState();
        }

        private void NextClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor14Planet3));
        }

        private void BackClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor12Planet1));
        }

        private void StagingClicked()
        {
            KeepSceneLoaded = false;
            Owner.ChangeState(new Instructor.DetectionState05(PartClassID, States.Instructor13Planet2));
        }
    }
}