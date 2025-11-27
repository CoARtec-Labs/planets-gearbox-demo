using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using CoARtec.UI.Dynamic.Data;

namespace CoARtec.UI.Dynamic.Infrastructure
{
    /// <summary>
    /// Calls coartec_api endpoints and parses JSON into DTOs.
    /// </summary>
    public class AssemblyProcedureApiClient : MonoBehaviour
    {
        [Header("API")]
        [SerializeField] private string baseUrl = "http://localhost:30000";
        [SerializeField] private int timeoutSeconds = 15;

        /// <summary>Base URL like http://localhost:30000</summary>
        public void SetBaseUrl(string url) => baseUrl = url;

        public IEnumerator GetProcedure(
            string procedureId,
            Action<AssemblyProcedureDto> onSuccess,
            Action<string> onError)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                onError?.Invoke("BaseUrl is empty.");
                yield break;
            }

            if (string.IsNullOrWhiteSpace(procedureId))
            {
                onError?.Invoke("ProcedureId is empty.");
                yield break;
            }

            string url = $"{baseUrl.TrimEnd('/')}/assembly-procedures/{UnityWebRequest.EscapeURL(procedureId)}";

            using var req = UnityWebRequest.Get(url);
            req.timeout = timeoutSeconds;

            // If you need auth later, add headers here:
            // req.SetRequestHeader("Authorization", "Bearer ...");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke($"HTTP error: {req.responseCode} - {req.error}\nURL: {url}");
                yield break;
            }

            var json = req.downloadHandler.text;
            if (string.IsNullOrWhiteSpace(json))
            {
                onError?.Invoke("Empty response body.");
                yield break;
            }

            try
            {
                var dto = JsonConvert.DeserializeObject<AssemblyProcedureDto>(json);

                if (dto == null)
                {
                    onError?.Invoke($"JSON parsed to null.\nBody:\n{json}");
                    yield break;
                }

                if (string.IsNullOrWhiteSpace(dto.id))
                {
                    onError?.Invoke($"DTO missing 'id'.\nBody:\n{json}");
                    yield break;
                }

                if (dto.steps == null)
                {
                    onError?.Invoke($"DTO missing 'steps'.\nBody:\n{json}");
                    yield break;
                }

                onSuccess?.Invoke(dto);
            }
            catch (Exception ex)
            {
                onError?.Invoke($"JSON parse error: {ex.Message}\nBody:\n{json}");
            }
        }
    }
}
