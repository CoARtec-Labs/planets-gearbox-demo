using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using coartec;
using coartec.BBox2DToolkit;
using coartec.MediaDisplay;
using UnityEngine.Serialization;

namespace coartec 
{
    public class DetectionManager : MonoBehaviour
    {
        public MeshRenderer imageDisplay;
        public YoloDetectorManager yoloDetector;
        public StagingView stagingView;
        
        public RectTransform detectionArea = null;
        
        [SerializeField, Tooltip("JSON file with bounding box color maps")]
        private TextAsset jsonColormapFile;
        
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
        
        // List to store label and color pairs for each class
        private List<(string, Color)> _colormapList = new List<(string, Color)>();
        
        private BoundingBox2DVisualizerMod _bBoxVisualizer;
        
        private BBox2D[] _bBoxDetections = null;
        private BBox2DInfo[] _bBoxInfoDetections = null;

        
        private void Awake()
        {
            Debug.Log("Set up references to BoundingBox2DVisualizer");
            _bBoxVisualizer = GetComponent<BoundingBox2DVisualizerMod>();
            
            if (_bBoxVisualizer == null)
                throw new System.ArgumentNullException("BoundingBox2DVisualizer not found.");
        }

        private void Start()
        {
            LoadColorMapList();
            
        }

        /// <summary>
        /// Runs the object detection on the image under display.
        /// </summary>
        public void TriggerDetection()
        {
            Debug.Log("This is a prediction action");

            Texture2D imageTexture;
            
            // The displayed image can be standard texture or webcam texture.
            if (imageDisplay.material.mainTexture.GetType() == typeof(WebCamTexture))
            {
                var webcam = (WebCamTexture)imageDisplay.material.mainTexture;
                imageTexture = new Texture2D(webcam.width, webcam.height);
                imageTexture.SetPixels(webcam.GetPixels());
                imageTexture.Apply();
            }
            else if (imageDisplay.material.mainTexture.GetType() == typeof(Texture2D))
            {
                imageTexture = imageDisplay.material.mainTexture as Texture2D;
            }
            else
            {
                throw new System.Exception("ImageDisplay.material.mainTexture must be WebCamTexture or Texture2D.");
            }

            if (stagingView.searchObjectClassId < 0)
            {
                _bBoxDetections = yoloDetector.Detect(imageTexture);
            }
            else
            {
                _bBoxDetections = yoloDetector.DetectClassId(imageTexture, stagingView.searchObjectClassId );
            }

            if (_bBoxDetections.Length > 0)
            {
                Debug.Log("Found " + _bBoxDetections.Length + " bBox points");
            }

            // Turn bbox array into bbox info array and change from image cos to  screen cos (flip y axis).
            _bBoxInfoDetections = _bBoxDetections.ToList()
                .Select(bbox => new BBox2DInfo(
                    new BBox2D(bbox.x0, bbox.y0, bbox.width, bbox.height, bbox.index, bbox.prob)))
                .ToArray();
            
            Vector2Int imageDims = new Vector2Int(imageTexture.width, imageTexture.height);
            
            UpdateBBoxDimensions(imageDims);
            UpdateBBoxColors();
            
            _bBoxVisualizer.UpdateBoundingBoxVisualizations(_bBoxInfoDetections);

            UpdateDetectionArea();
        }

        private void UpdateDetectionArea()
        {
            // update the size of the detection area marker
            detectionArea.localPosition = new Vector3(0, 0, 0);

            // Check if the screen is mirrored
            bool mirrorScreen = imageDisplay.transform.localScale.z == -1;

            // Get the screen dimensions
            Vector2 screenDims = new Vector2(imageDisplay.transform.localScale.x,
                imageDisplay.transform.localScale.y);

            Vector2Int targetDims = new Vector2Int(yoloDetector.ImageWidth,
                yoloDetector.ImageHeight);

            var imageDims = new Vector2Int(imageDisplay.material.mainTexture.width,
                imageDisplay.material.mainTexture.height);

            var offset = (imageDims - targetDims) / 2;

            BBox2D bbox = BBox2DUtility.ScaleBoundingBox(
                new BBox2D(0, 0, 320, 320, -1, 0),
                imageDims, screenDims, offset, mirrorScreen);

            detectionArea.sizeDelta = new Vector2(bbox.width, bbox.height);

            //Debug.Log($"Screen.Width, Height: {Screen.width}, {Screen.height}");
        }

        private void UpdateBBoxColors()
        {
            for (var i = 0; i < _bBoxInfoDetections.Length; i++)
            {
                // var detection = detections[i];
                var idx = _bBoxInfoDetections[i].bbox.index;

                _bBoxInfoDetections[i].label = _colormapList[idx].Item1;
                _bBoxInfoDetections[i].color = _colormapList[idx].Item2;
            }
        }
        
        /// Update predictions based on image dimensions and screen dimensions.
        private void UpdateBBoxDimensions(Vector2Int imageDims)
        {
            // Check if the screen is mirrored
            bool mirrorScreen = imageDisplay.transform.localScale.z == -1;

            // Get the screen dimensions
            Vector2 screenDims = new Vector2(imageDisplay.transform.localScale.x, 
                imageDisplay.transform.localScale.y);
            
            Vector2Int targetDims = new Vector2Int(yoloDetector.ImageWidth, 
                yoloDetector.ImageHeight);
            
            var offset = (imageDims - targetDims) / 2;

            // offset.x = 0;
            // offset.y = 0;

            // Scale and position the bounding boxes based on the input and screen dimensions
            for (int i = 0; i < _bBoxInfoDetections.Length; i++)
            {
                _bBoxInfoDetections[i].bbox = BBox2DUtility.ScaleBoundingBox(
                    _bBoxInfoDetections[i].bbox, imageDims, screenDims, offset, mirrorScreen);
            }
        }
        
        // Load the color map list from the JSON file
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
        private bool IsColorMapListJsonNullOrEmpty()
        {
            return jsonColormapFile == null || string.IsNullOrWhiteSpace(jsonColormapFile.text);
        }

        // Deserialize the color map list from the JSON string
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
