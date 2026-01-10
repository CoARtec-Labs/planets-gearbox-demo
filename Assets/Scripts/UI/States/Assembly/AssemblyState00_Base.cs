using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.States.Assembly
{
    /// <summary>
    /// This is the assembly state.
    /// </summary>
    public class AssemblyState00_Base : BaseState
    {
        private const int PartClassID = -1; // all parts

        // Used to set scene loading on or off
        // public bool KeepSceneLoaded = false;

        public AssemblyState00_Base()
        {
            SceneName = "Assembly";
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
            Owner.ChangeState(new Detection.DetectionState(PartClassID, States.Assembly00Base));
        }

        private void NextClicked()
        {
            // Debug.Log("[AssemblyState00_Base.cs] Next clicked.");

            KeepSceneLoaded = true;
            Owner.ChangeState(new AssemblyState10_Ring());
        }


    }
}