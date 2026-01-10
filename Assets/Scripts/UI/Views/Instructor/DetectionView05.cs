using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Views.Instructor
{
    /// <summary>
    /// The StagingView class.
    /// This class is used to (de)activate the staging UI.
    /// </summary>
    public class DetectionView05 : BaseViewSingleton<DetectionView05>
    {
        // Events to attach to:
        public static UnityAction OnAssemblyClicked;
    
        public static int SearchObjectClassId = -1;

        [SerializeField] private TMP_Text searchTitleBanner;
        [SerializeField] private ARActivationController arController;
        [SerializeField] private GameObject partsDetector;
    
        [Header("References to files")]
        [SerializeField, Tooltip("JSON file with bounding box color maps")]
        private TextAsset jsonColormapFile;

        private List<(string, Color)> _colormapList = new();
    
        // Serializable class to store color map information
        [System.Serializable]
        class Colormap
        {
            public string label;
            public List<float> color;
        }

        // Serializable class to store multiple color maps.
        [System.Serializable]
        class ColormapList
        {
            public List<Colormap> items;
        }
    
        private new void Awake()
        {
            base.Awake();
        
            LoadColorMapList();
        }

        private void OnEnable()
        {
            arController.PauseAR();
            partsDetector.SetActive(true);
        
            string objectName;
        
            if (SearchObjectClassId == -1)
            {
                objectName = "example";
            }
            else
            {
                objectName = _colormapList[SearchObjectClassId].Item1;
            }
        
            searchTitleBanner.GetComponent<TMP_Text>().SetText($"Suche: {objectName}");
        }

        private void OnDisable()
        {
            arController.ResumeAR();
            partsDetector.SetActive(false);
        }
    
        /// <summary>
        /// Method for assembly button.
        /// </summary>
        public void AssemblyClick()
        {
            OnAssemblyClicked?.Invoke();
        }
    
        // Load the color map list from the JSON file
        // TODO remove code redundancy with detection manager
        private void LoadColorMapList()
        {
            if (IsColorMapListJsonNullOrEmpty())
            {
                Debug.LogError("Class labels JSON is null or empty.");
                return;
            }

            ColormapList colormapObj = DeserializeColorMapList(jsonColormapFile.text);
            UpdateColorMap(colormapObj);
        }
    
        // Check if the color map JSON file is null or empty
        // TODO remove code redundancy with detection manager
        private bool IsColorMapListJsonNullOrEmpty()
        {
            return jsonColormapFile == null || string.IsNullOrWhiteSpace(jsonColormapFile.text);
        }

        // Deserialize the color map list from the JSON string
        // TODO remove code redundancy with detection manager
        private ColormapList DeserializeColorMapList(string json)
        {
            try
            {
                return JsonUtility.FromJson<ColormapList>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to deserialize class labels JSON: {ex.Message}");
                return null;
            }
        }
    
        // Update the color map list with deserialized data
        // TODO remove code redundancy with detection manager
        private void UpdateColorMap(ColormapList colormapObj)
        {
            if (colormapObj == null)
            {
                return;
            }

            // Add label and color pairs to the colormap list
            foreach (Colormap colormap in colormapObj.items)
            {
                Color color = new Color(colormap.color[0], colormap.color[1], colormap.color[2]);
                _colormapList.Add((colormap.label, color));
            }
        }
    }
}
