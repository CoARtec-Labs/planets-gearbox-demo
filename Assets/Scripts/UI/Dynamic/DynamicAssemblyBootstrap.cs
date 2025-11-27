using UnityEngine;
using CoARtec.UI.Dynamic.Data;
using CoARtec.UI.Dynamic.Infrastructure;

namespace CoARtec.UI.Dynamic
{
    /// <summary>
    /// Scene entry point for dynamic assembly:
    /// Loads a procedure from API and initializes AssemblySceneController.
    /// </summary>
    public class DynamicAssemblyBootstrap : MonoBehaviour
    {
        [Header("Scene references")]
        [SerializeField] private AssemblySceneController controller;
        [SerializeField] private AssemblyProcedureApiClient apiClient;

        [Header("API configuration")]
        [Tooltip("Example: http://localhost:30000 (Editor same machine). On mobile use http://<PC_IP>:30000")]
        [SerializeField] private string baseUrl = "http://localhost:30000";

        [Tooltip("Example: gearbox_v1")]
        [SerializeField] private string procedureId = "gearbox_v1";

        private void Start()
        {
            if (controller == null)
                controller = FindObjectOfType<AssemblySceneController>();

            if (apiClient == null)
                apiClient = FindObjectOfType<AssemblyProcedureApiClient>();

            if (controller == null)
            {
                Debug.LogError("[DynamicAssemblyBootstrap] AssemblySceneController not found in scene.");
                return;
            }

            if (apiClient == null)
            {
                // Auto-create client if not placed in scene
                var go = new GameObject("AssemblyProcedureApiClient");
                apiClient = go.AddComponent<AssemblyProcedureApiClient>();
            }

            apiClient.SetBaseUrl(baseUrl);

            Debug.Log($"[DynamicAssemblyBootstrap] Loading procedure '{procedureId}' from {baseUrl} ...");

            StartCoroutine(apiClient.GetProcedure(
                procedureId,
                OnLoaded,
                OnError
            ));
        }

        private void OnLoaded(AssemblyProcedureDto dto)
        {
            Debug.Log($"[DynamicAssemblyBootstrap] Loaded procedure '{dto.id}' with {dto.steps.Count} steps.");
            controller.InitializeFromDto(dto);
        }

        private void OnError(string err)
        {
            Debug.LogError($"[DynamicAssemblyBootstrap] Failed to load procedure: {err}");
        }
    }
}
