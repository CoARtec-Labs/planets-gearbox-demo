using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Assembly
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState16_Gasket : BaseState
    {
        private const int PartClassID = 7; // gasket-gold 

        public AssemblyState16_Gasket()
        {
            SceneName = "Assembly";
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
            Owner.ChangeState(StateFactory.CreateState(States.Assembly15Carrier));
        }

        private void BackClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Assembly17Lid));
        }

        private void StagingClicked()
        {
            KeepSceneLoaded = false;
            Owner.ChangeState(new Detection.DetectionState(PartClassID, States.Assembly16Gasket));
        }

    }
}