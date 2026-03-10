using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

using UI.Views;
using UI.Views.Instructor;

namespace UI.States.Instructor
{
    /// <summary>
    /// Staging state: detection and highlighting of assembly parts.
    /// </summary>
    public class DetectionState05 : BaseState
    {
        // Label of the assembly object to be searched for
        private readonly int _searchObjectClassId;
        private readonly States _returningState;

        public DetectionState05()
        {
            KeepSceneLoaded = false;
            SceneName = "Instructor";
            
            _returningState = States.NONE;
            _searchObjectClassId = -1;
        }

        public DetectionState05(int objectClassId) : this()
        {
            _searchObjectClassId = objectClassId;
        }

        public DetectionState05(int objectClassId, States returningState) : this()
        {
            _searchObjectClassId = objectClassId;
            _returningState = returningState;
        }

        public override void PrepareState()
        {
            base.PrepareState();

            UIRootBase.SearchObjectClassId = _searchObjectClassId;
            
            DetectionView05.SearchObjectClassId = _searchObjectClassId;
            DetectionView05.OnAssemblyClicked += AssemblyClicked;

            DetectionView05.Instance.ShowView();
        }

        public override void DestroyState()
        {
            DetectionView05.Instance.HideView();
            DetectionView05.OnAssemblyClicked -= AssemblyClicked;
            DetectionView05.SearchObjectClassId = -1;

            base.DestroyState();
        }

        private void AssemblyClicked()
        {
            Debug.Log("[StagingState.cs] assembly clicked.");

            KeepSceneLoaded = true;

            if (_returningState == States.NONE)
            {
                Owner.ChangeState(StateFactory.CreateState(States.Assembly00Base));
            }
            else
            {
                Owner.ChangeState(StateFactory.CreateState(_returningState));
            }
        }
    }
}
