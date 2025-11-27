using System;
using System.Collections.Generic;
using UnityEngine;
using CoARtec.UI.Dynamic.Data;
using CoARtec.UI.Dynamic.Runtime;
using CoARtec.UI.Dynamic.Views;

namespace CoARtec.UI.Dynamic
{
    public class AssemblySceneController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private PartRegistry partRegistry;
        [SerializeField] private DynamicAssemblyView view;

        private AssemblyProcedureRuntime _runtime;

        public void InitializeFromDto(AssemblyProcedureDto dto)
        {
            _runtime = new AssemblyProcedureRuntime(dto);

            // View-Events hookup
            if (view != null)
            {
                view.OnNextClicked.AddListener(OnNext);
                view.OnBackClicked.AddListener(OnBack);
                view.OnStagingClicked.AddListener(OnStaging);
            }

            ApplyCurrentStep();
        }

        private void OnDestroy()
        {
            if (view != null)
            {
                view.OnNextClicked.RemoveListener(OnNext);
                view.OnBackClicked.RemoveListener(OnBack);
                view.OnStagingClicked.RemoveListener(OnStaging);
            }
        }

        private void OnNext()
        {
            if (_runtime == null) return;
            if (_runtime.CanGoNext)
            {
                _runtime.GoNext();
                ApplyCurrentStep();
            }
        }

        private void OnBack()
        {
            if (_runtime == null) return;
            if (_runtime.CanGoBack)
            {
                _runtime.GoBack();
                ApplyCurrentStep();
            }
        }

        private void OnStaging()
        {
            // TODO: Change with detection state _runtime.CurrentStep.YoloClassId
            Debug.Log($"[AssemblySceneController] Staging requested for step '{_runtime?.CurrentStep?.Id}', yoloClassId={_runtime?.CurrentStep?.YoloClassId}");
        }

        private void ApplyCurrentStep()
        {
            if (_runtime == null) return;
            var step = _runtime.CurrentStep;

            //UI update
            if (view != null)
                view.SetStepText(step.Title, step.Description);

            //Highlighting parts
            UpdateVisibleParts(step.VisiblePartKeys, step.HighlightPartKeys);
        }

        private void UpdateVisibleParts(string[] visibleKeys, string[] highlightKeys)
        {
            var visible = new HashSet<string>(visibleKeys ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            var highlights = new HashSet<string>(highlightKeys ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

            foreach (var kv in partRegistry.Enumerate())
            {
                var key = kv.Key;
                var go  = kv.Value;
                var show = visible.Contains(key);

                if (go) go.SetActive(show);

                // TODO: optional highlighting: etc. Outline/Material change
                // if (show && highlights.Contains(key))...
            }
        }
    }
}
