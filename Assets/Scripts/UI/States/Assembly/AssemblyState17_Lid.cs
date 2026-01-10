using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Assembly
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState17_Lid : BaseState
    {
        private const int PartClassID = 0; // lid-grey

        public AssemblyState17_Lid()
        {
            SceneName = "Assembly";
        }

        public override void PrepareState()
        {
            base.PrepareState();

            AssemblyView17_Lid.OnBackClicked += BackClicked;
            AssemblyView17_Lid.OnStagingClicked += StagingClicked;
            AssemblyView17_Lid.Instance.ShowView();
        }

        public override void DestroyState()
        {
            AssemblyView17_Lid.Instance.HideView();
            AssemblyView17_Lid.OnBackClicked -= BackClicked;
            AssemblyView17_Lid.OnStagingClicked -= StagingClicked;

            base.DestroyState();
        }

        private void BackClicked()
        {
            KeepSceneLoaded = true;
            Owner.ChangeState(StateFactory.CreateState(States.Assembly16Gasket));
        }

        private void StagingClicked()
        {
            // Debug.Log("[AssemblyStateStepBase.cs] Staging clicked.");

            KeepSceneLoaded = false;
            Owner.ChangeState(new Detection.DetectionState(PartClassID, States.Assembly17Lid));
        }
    }
}