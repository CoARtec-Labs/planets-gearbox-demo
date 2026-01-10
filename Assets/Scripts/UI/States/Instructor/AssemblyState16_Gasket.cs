using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Instructor
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState16_Gasket : BaseState
    {
        private const int PartClassID = 7; // gasket-gold 

        public AssemblyState16_Gasket()
        {
            SceneName = "Instructor";
        }

        public override void PrepareState()
        {
            base.PrepareState();

            AssemblyView16_Gasket.OnNextClicked += NextClicked;
            AssemblyView16_Gasket.OnBackClicked += BackClicked;
            AssemblyView16_Gasket.OnStagingClicked += StagingClicked;

            AssemblyView16_Gasket.Instance.ShowView();
        }

        public override void DestroyState()
        {
            AssemblyView16_Gasket.Instance.HideView();
            AssemblyView16_Gasket.OnNextClicked -= NextClicked;
            AssemblyView16_Gasket.OnBackClicked -= BackClicked;
            AssemblyView16_Gasket.OnStagingClicked -= StagingClicked;

            base.DestroyState();
        }

        private void NextClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor17Lid));
        }

        private void BackClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Instructor15Carrier));
        }

        private void StagingClicked()
        {
            KeepSceneLoaded = false;
            Owner.ChangeState(new Instructor.DetectionState05(PartClassID, States.Instructor16Gasket));
        }

    }
}