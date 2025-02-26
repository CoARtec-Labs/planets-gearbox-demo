using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using coartec;
using coartec.BBox2DToolkit;

namespace coartec 
{
    public class DetectionManager : MonoBehaviour
    {
        public MeshRenderer ImageDisplay;
        public ObjectDetectorManager ObjectDetector;
        
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
        private List<(string, Color)> colormapList = new List<(string, Color)>();
        
        private BoundingBox2DVisualizerMod bBoxVisualizer;
        private BBox2DInfo[] detections = null;

        
        private void Awake()
        {
            Debug.Log("Set up references to BoundingBox2DVisualizer");
            bBoxVisualizer = GetComponent<BoundingBox2DVisualizerMod>();
            
            if (bBoxVisualizer == null)
                throw new System.ArgumentNullException("BoundingBox2DVisualizer not found.");
        }

        private void Start()
        {
            LoadColorMapList();
        }

        // Runs the object detection on the image under display.
        public void TriggerDetection()
        {
            Debug.Log("This is a prediction action");

            Texture2D imageTexture;
            
            // The displayed image can be standard texture or webcam texture.
            if (ImageDisplay.material.mainTexture.GetType() == typeof(WebCamTexture))
            {
                var webcam = (WebCamTexture)ImageDisplay.material.mainTexture;
                imageTexture = new Texture2D(webcam.width, webcam.height);
                imageTexture.SetPixels(webcam.GetPixels());
                imageTexture.Apply();
            }
            else if (ImageDisplay.material.mainTexture.GetType() == typeof(Texture2D))
            {
                imageTexture = ImageDisplay.material.mainTexture as Texture2D;
            }
            else
            {
                throw new System.Exception("ImageDisplay.material.mainTexture must be WebCamTexture or Texture2D.");
            }
            
            detections = ObjectDetector.Detect(imageTexture);
            
            Vector2Int imageDims = new Vector2Int(imageTexture.width, imageTexture.height);

            //UpdateBBoxDimensions(imageDims);
            UpdateBBoxColors();
            
            bBoxVisualizer.UpdateBoundingBoxVisualizations(detections);
        }

        private void UpdateBBoxColors()
        {
            for (var i = 0; i < detections.Length; i++)
            {
                // var detection = detections[i];
                var idx = detections[i].bbox.index;

                detections[i].label = colormapList[idx].Item1;
                detections[i].color = colormapList[idx].Item2;
            }
        }
        
        /// Update predictions based on image dimensions and screen dimensions.
        private void UpdateBBoxDimensions(Vector2Int imageDims)
        {
            // Check if the screen is mirrored
            bool mirrorScreen = ImageDisplay.transform.localScale.z == -1;

            // Get the screen dimensions
            Vector2 screenDims = new Vector2(ImageDisplay.transform.localScale.x, 
                ImageDisplay.transform.localScale.y);
            
            Vector2Int targetDims = new Vector2Int(ObjectDetector.ImageWidth, 
                ObjectDetector.ImageHeight);
            
            var offset = (imageDims - targetDims) / 2;

            // Scale and position the bounding boxes based on the input and screen dimensions
            for (int i = 0; i < detections.Length; i++)
            {
                detections[i].bbox = BBox2DUtility.ScaleBoundingBox(
                    detections[i].bbox, imageDims, screenDims, offset, mirrorScreen);
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
                colormapList.Add((colormap.label, color));
            }
        }
    }
}
